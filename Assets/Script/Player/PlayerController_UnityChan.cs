using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController_UnityChan : MonoBehaviour
{
    [SerializeField] // This will make the variable below appear in the inspector
    float playerSpeed = 9;
    [SerializeField] // This will make the variable below appear in the inspector
    public float airspeed = 12;
    [SerializeField]
    public float jumpHeight = 10;
    [SerializeField]
    float gravityValue = 30;
    [HideInInspector]
    public Vector3 moveDirection = Vector3.zero;
    Vector3 move = Vector3.zero;
    public Vector2 pushBack = Vector2.zero;
    Vector3 rotation = Vector3.zero;
    public CharacterController controller;
    MeshRenderer material;
    [SerializeField]
    public SceneCamera sceneCamera;
    bool isWalking = false;
    bool isGrounded;
    public bool paused;

    //bool isJumping; // "controller.isGrounded" can be used instead
    [SerializeField]
    int nrOfAlowedDJumps = 2; // New vairable
    int dJumpCounter = 0;     // New variable
    bool doubleJump = false;

    // this make sure to know change in direction and that way divide airspeed by 2
    public bool right;
    public bool left;

    // coyote time
    [SerializeField]
    float coyoteTime = 0.2f;
    float coyoteCounter;

    //Resource Variables
    public int HP = 100;
    private int maxHP;
    public int Mana = 100;
    private int maxMana;
    //Hurt variables
    private bool frozen;
    private float launch;
    [SerializeField] private float launchRecovery;
    [SerializeField] private Vector3 launchPower;


    //Respown Variables//WIP
    [HideInInspector]
    public Vector3 respawnPosition;
    [HideInInspector]
    public bool haveCheckPoint = false;

    //Animations Variables
    [SerializeField] internal Animator animator;
    internal string currentState;
    bool haveSword = false;
    bool comboPossible;
    int comboStep;
    [SerializeField] public GameObject drawSword;
    [SerializeField] public GameObject unDrawSword;


    //collision variables
    internal RaycastHit hit;
    internal bool raycastHit;
    [SerializeField]
    int rayRange;

    //Dash Variables
    [SerializeField]
    float dashTime = 0.1f;
    [SerializeField]
    float dashSpeed = 30.0f;
    [SerializeField]
    //float coolDowntDashTime = 1f;

    //Attack Variables
    public float attackDuration;
    [HideInInspector]
    public int attackPower;
    public int attack = 25;
    private bool isAttacking = false;
    private bool canAttack = true;
    public int attackRange = 3;
    [SerializeField] GameObject attackWeapon;
    public float weaponStartRotation = 15;

    public Vector3 spawnLocation;
    public bool playerIsDead = false;

    public Collider swordCollider;

    //WallJump Variables
    public float wallMultiplier = 0.75F;
    public float wallJumpSpeed = 6.0F;
    private float currentWallMultiplier;
    private bool wallCollision;
    bool isWallSliding;
    bool isTouchingWall;
    public LayerMask Wall;
    public float wallSlideSpeed=3;
    public int wallrayRange = 1;
    int faceDirection;
    bool wallJumping;
    public int WJumpCounter;

    //Animator Functions
    public AudioSource sfxAudioSource;
    public ParticleSystem ParticuleSource;
    //public GameObject animatorFunctions;
    bool playAnimation;
    int playAnimationSpeed = 6;

    public float coolDownTime = 4;
    private float nextFireTime = 0;

    public GameObject particuleDead;
    public GameObject particuleHit;
    public GameObject particuleAttack;
    public GameObject particuleDust;
    public GameObject WeponTrail;
    public GameObject DashTrail;
    public GameObject particuleSCH;



    public Dictionary<string, Sprite> inventory = new Dictionary<string, Sprite>(); //Dictionary storing all inventory item strings and values
    //public Sprite inventoryItemBlank; //The default inventory item slot sprite
    //public Sprite keySprite; //The key inventory item
    //public Sprite keyGemSprite; //The gem key inventory item

    private bool isDshing=false;
    Vector3 movedash;

    Bounds box;

    void Start()
    {
        
        maxHP = HP;
        maxMana = Mana;
        material = GetComponent<MeshRenderer>();
        controller = gameObject.GetComponent<CharacterController>();
        drawSword.SetActive(true);
        unDrawSword.SetActive(false);
        haveSword = true;

        spawnLocation = GameObject.FindGameObjectWithTag("Respawn").transform.position;
        transform.position = spawnLocation;
        HUDScript.pauseGame += Pause;
        box = new Bounds(Vector3.zero, ((Vector3.right * 1500) + (Vector3.up * 1500) + (Vector3.forward*200)));
    }
    void Update()
    {
        if (!paused)
        {
            if (HP > maxHP) {
                HP = maxHP;
            }
            if (Mana > maxMana) {
                Mana = maxMana;
            }
            //transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            if (!playerIsDead)
            {
               
                isGrounded = controller.isGrounded;
                //Gamemanager.Instance.playerScript.ParticuleSource = AnimatorFunctions.Instance.playStepEmitParticles;
                pushBack = Vector2.Lerp(pushBack, Vector2.zero, 6 * Time.deltaTime);

                //Raycast hits bools
                raycastHit = Physics.Raycast(transform.position, transform.forward, out hit, rayRange);
                isTouchingWall = Physics.Raycast(transform.position, transform.forward, out hit, wallrayRange, Wall);

                animator.SetFloat("speed", Mathf.Abs(move.x));

                animator.SetBool("ground", controller.isGrounded);


                //animator.SetBool("grounded", controller.isGrounded);
                //animator.SetFloat("velocityY", moveDirection.y);
                //animator.SetFloat("velocityX", move.x);
                //animator.SetBool("IsWalking", isWalking);

                if (controller.isGrounded && moveDirection.y <= 0)
                {
                    moveDirection.y = 0f;
                }
                animator.SetFloat("speed.y", moveDirection.y);

                move = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
                //controller.Move(move * Time.deltaTime * playerSpeed);

                if (move != Vector3.zero)
                {
                    gameObject.transform.forward = move;
                }   
                //else if (isDshing)
                //{
                //    if (gameObject.transform.forward.x >=0)
                //    {

                //        move.x = 1;
                //    }
                //    else
                //    {
                //        move.x = -1;
                //    }
                //}
                //if (move == Vector3.zero)
                //{
                //    rotation = new Vector3(0,180,0);
                //    gameObject.transform. = new Vector3(0, 180, 0);
                //}
                // Dash();
                if (controller.isGrounded)
                {
                    move.x = Input.GetAxis("Horizontal") * playerSpeed;
                    //move.x = Mathf.Lerp(0, Input.GetAxis("Horizontal") * playerSpeed, 4f);
                    //controller.Move(move * Time.deltaTime * playerSpeed);
                    //moveDirection.x = Input.GetAxis("Horizontal") * playerSpeed;
                    right = false;
                    left = false;
                    //isWalking = false;
                    coyoteCounter = coyoteTime;
                    dJumpCounter = 0;
                    doubleJump = false;
                }
                else if (!controller.isGrounded)
                {
                    if (move.x >= 0)
                    {
                        faceDirection = 1;
                        move.x = Input.GetAxis("Horizontal") * airspeed;

                        ////debug.log("right");
                        right = true;
                        //isWalking = true;
                        if (left)
                        {
                            faceDirection = -1;
                            if (!wallJumping)
                                move.x = Input.GetAxis("Horizontal") * airspeed / 2;
                            //controller.Move((move * Time.deltaTime * airspeed)/2);
                            ////debug.log("Change left");
                            //moveDirection.x = Input.GetAxis("Horizontal") * airspeed / 2;
                            right = false;

                        }

                    }
                    else if (move.x <= 0)
                    {
                        faceDirection = -1;
                        move.x = Input.GetAxis("Horizontal") * airspeed;
                        ////debug.log("left");
                        left = true;
                        //isWalking = true;
                        if (right)
                        {
                            faceDirection = 1;
                            if (!wallJumping)
                                move.x = Input.GetAxis("Horizontal") * airspeed / 2;
                            //controller.Move((move * Time.deltaTime * airspeed)/2);
                            ////debug.log("Change right");
                            //moveDirection.x = Input.GetAxis("Horizontal") * airspeed / 2;
                            left = false;
                        }

                    }
                    coyoteCounter -= Time.deltaTime;
                }
                if (Input.GetAxis("LaserHold") < 0)
                {
                    move.x = 0;
                }

                //controller.Move((move + (Vector3)pushBack) * Time.deltaTime);

                if (Input.GetButtonDown("Jump") && !playAnimation)
                {
                    if (controller.isGrounded && coyoteCounter > 0 && dJumpCounter == 0)
                    {
                        animator.SetTrigger("jump");
                        Gamemanager.Instance.animatorFunctionsScript.PlayJumpSound();
                        //Gamemanager.Instance.animatorFunctionsScript.PlayStepEmitParticles();
                        GameObject newparticule = (GameObject)Instantiate(particuleDust, transform.position, Quaternion.identity);
                        Destroy(newparticule, 1);
                        moveDirection.y = jumpHeight;
                        dJumpCounter ++;
                        //controller.transform.position = new Vector3(transform.position.x, transform.position.y + 10, 0);
                    }
                    if (!controller.isGrounded && dJumpCounter < nrOfAlowedDJumps)
                    {

                        animator.SetTrigger("doubleJump");
                        Gamemanager.Instance.animatorFunctionsScript.PlayJumpSound();
                        Gamemanager.Instance.animatorFunctionsScript.PlayStepEmitParticles();
                        moveDirection.y = jumpHeight;
                        dJumpCounter++;
                    }
                }


                if (!controller.isGrounded)
                {
                    
                    //player fall or gravity
                    if (isWallSliding)
                    {
                        //moveDirection = new Vector3(moveDirection.x, 0, 0);
                        moveDirection = new Vector3(moveDirection.x, -wallSlideSpeed, 0);
                        print("isWallSliding");
                    }
                    else if (wallCollision)
                    {
                        moveDirection.y -= gravityValue * Time.deltaTime * currentWallMultiplier;
                    }
                    else
                    {
                        moveDirection.y -= gravityValue * Time.deltaTime /** currentWallMultiplier*/;
                    }
                }

                //moveDirection.y -= gravityValue * Time.deltaTime * currentWallMultiplier;

                //movement of the player
                //CollisionFlags Movecollision = controller.Move(moveDirection * Time.deltaTime);

                //fix stuck above
                if ((controller.collisionFlags & CollisionFlags.Above) != 0)
                {
                    ////debug.log("player on slides");
                    moveDirection.y -= 1;
                }

                //if ((controller.collisionFlags & CollisionFlags.CollidedSides) != 0)
                //{
                //    //debug.log("player on slides");
                //    moveDirection.y -= 1;
                //}

                
                
                if (Input.GetButtonDown("DrawSword"))
                {
                    if (!haveSword)
                    {
                        haveSword = true;
                        ////debug.log("haveSword = true");
                    }
                    else
                    {
                        ////debug.log("haveSword = false");
                        haveSword = false;
                    }
                    animator.SetBool("haveSword", haveSword);
                    animator.SetTrigger("DrawSword");

                }

                if ( (Input.GetAxis("Vertical") > 0.1f  && Input.GetButtonDown("Fire1") || (Input.GetButtonDown("SJ")))  && !playAnimation && haveSword)
                {
                    SJ();
                   
                }
                if (Mana >= 10)
                {
                    if ((Mathf.Round(Input.GetAxisRaw("SCH")) > 0 && !playAnimation && haveSword))
                    {
                        animator.SetTrigger("SCH");
                        //Gamemanager.Instance.animatorFunctionsScript.PlayOnHitSound();
                        GameObject newparticule = (GameObject)Instantiate(particuleSCH, transform.position + new Vector3(Random.Range(0, 2), Random.Range(1, 3)), Quaternion.identity);
                        Destroy(newparticule, 1);
                        playAnimationSpeed = 2;
                        attackPower = 30;
                        Mana -= 10;
                        StartCoroutine(AtivateAttack());
                    }
                }
               
                if (Input.GetButtonDown("SD") && !playAnimation && haveSword)
                {
                    animator.SetTrigger("SD");
                    //Gamemanager.Instance.animatorFunctionsScript.PlayOnHitSound();
                    //GameObject newparticule = (GameObject)Instantiate(particuleHit, transform.position + new Vector3(Random.Range(0, 2), Random.Range(1, 3)), Quaternion.identity);
                    //Destroy(newparticule, 1);
                    attackPower = 20;
                    playAnimationSpeed = 4;
                    StartCoroutine(AtivateAttack());
                }

                CheckIfWallSliding();
                //function
                Health();
                //UpdateAnimations();
                //RespawnPosition();
                Attack();
                //transform.position.Set(transform.position.x, transform.position.y, 0);

                if (playAnimation)
                {
                    move.x = Input.GetAxis("Horizontal") * playAnimationSpeed / 2;
                }
                else
                {
                    //checks
                    attackPower = attack;
                }

                //if (move == Vector3.zero)
                //{
                //    if (isDshing)
                //    {
                //        if (gameObject.transform.forward.x >= 0)
                //        {

                //            move.x = 1*playerSpeed;
                //        }
                //        else
                //        {
                //            move.x = -1 * playerSpeed;
                //        }
                //    }
                //}
                //if (Input.GetAxis("Vertical") > 0.1f && isDshing)
                //{
                //    move.y = 1 * playerSpeed;
                //    move.x = 0;
                //}
                if (Time.time > nextFireTime)
                    DashTrail.SetActive(false);
                if (Time.time > nextFireTime)
                {
                    if (Input.GetButtonDown("Dash"))
                    {
                        movedash = Vector3.zero;

                        if (move == Vector3.zero)
                        {
                            
                           
                                if (gameObject.transform.forward.x >= 0)
                                {

                                    movedash.x = 1 * playerSpeed;
                                }
                                else
                                {
                                    movedash.x = -1 * playerSpeed;
                                }
                            
                        }
                        if (Input.GetAxis("Vertical") > 0.1f)
                        {
                            movedash.y = 1 * playerSpeed;
                            movedash.x = 0;
                        }
                        if(Mathf.Abs (Input.GetAxis("Horizontal")) > 0)
                        {
                            movedash.x = Input.GetAxis("Horizontal") * playerSpeed;
                            //movedash.y = 0;
                        }
                       
                        animator.SetTrigger("dash");
                        StartCoroutine(DashCoroutine());
                        nextFireTime = Time.time + coolDownTime;
                    }
                }

                if (isDshing)
                {
                    //move *= dashSpeed;
                    move = movedash * dashSpeed;
                }
                else
                {
                    move += (Vector3)pushBack + moveDirection;
                }

               

                controller.Move((Vector2)move * Time.deltaTime);
                controller.transform.position = new Vector3(controller.transform.position.x, controller.transform.position.y, 0);
            }
        }
    }

    public void CoolDown()
    {

        //if (Input.GetButton("Fire1") && Time.time > NextFire)
        //{
        //    //If the player fired, reset the NextFire time to a new point in the future.
        //    NextFire = Time.time + FireRate;


        //    //Weapon firing logic goes here.


        //    //debug.log("Firing once every 1s");
        //}
    }
    public void SJ()
    {
        if (!controller.isGrounded && dJumpCounter < nrOfAlowedDJumps)
        {

            animator.SetTrigger("SJ");
            StartCoroutine(AtivateAttack());
            Gamemanager.Instance.animatorFunctionsScript.PlayJumpSound();
            Gamemanager.Instance.animatorFunctionsScript.PlayStepEmitParticles();
            moveDirection.y = jumpHeight+2;
            dJumpCounter++;
        }
    }
    public void AddInventoryItem(string inventoryName, Sprite image = null)
    {
        inventory.Add(inventoryName, image);
        //The blank sprite should now swap with key sprite
        //Gamemanager.Instance.inventoryItemImage
        //GameManager.Instance.inventoryItemImage.sprite = inventory[inventoryName];
    }

    public void RemoveInventoryItem(string inventoryName, bool removeAll = false)
    {
        if (!removeAll)
        {
            inventory.Remove(inventoryName);
        }
        else
        {
            inventory.Clear();
        }

        inventory.Remove(inventoryName);
        //The blank sprite should now swap with key sprite
        //GameManager.Instance.inventoryItemImage.sprite = inventoryItemBlank;
    }
    //WallJump
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if ((hit.normal.y < 0.1f && !controller.isGrounded /*&& isWallSliding*/&& !playAnimation) && (controller.collisionFlags & CollisionFlags.Above) == 0)
        {
            if(hit.gameObject.tag == "Wall")
            {
                //dJumpCounter = 1;
            }
            if (!controller.isGrounded &&  hit.normal.x > 0.1f)
            {
                dJumpCounter = 1;
            }
            //print("WallColided");
            currentWallMultiplier = wallMultiplier;
            //dJumpCounter = 1;
            
            wallCollision = true;
            if (Input.GetButtonDown("Jump") && !playAnimation /*&& WJumpCounter < 1*/)
            {
                dJumpCounter = 0;
                moveDirection = hit.normal * airspeed * 2 * wallJumpSpeed;
                moveDirection.y = jumpHeight;
                //print("walljump");
                WJumpCounter++;
            }
            //moveDirection.x = Mathf.Lerp(moveDirection.x, 0, 1f);
            moveDirection.x = 0;
            if (controller.isGrounded)
            {
                //print("wall in air");
                //moveDirection.x = Mathf.Lerp(moveDirection.x / 2, 0, .5f);
            }
        }
        else
        {
            if (!controller.isGrounded)
            {
                //dJumpCounter = 1;
            }
            moveDirection.x = 0;
            currentWallMultiplier = 1.0F;
            wallCollision = false;
            WJumpCounter = 0;
        }
    }

    private void CheckIfWallSliding()
    {
        if (isTouchingWall && move.x != 0)
        {
            StartCoroutine(AtivatWallSliding());
        }
        else
        {
            isWallSliding = false;
        }
    }
    private IEnumerator AtivatWallSliding()
    {
        float timer = .2f;
        while (timer > 0) {
            if (!paused) {
                timer -= Time.deltaTime;
            }
            yield return null;
        }
        //yield return new WaitForSeconds(.2f);
        isWallSliding = true;
    }

    private void Health()
    {
        if (!box.Contains(transform.position))
        {
            HP = 0;
        }
        //If health < 0, destroy me
        if (HP <= 0)
        {
            animator.SetTrigger("Dead");
            GameObject newparticule = (GameObject)Instantiate(particuleDead, transform.position, Quaternion.identity);
            Destroy(newparticule, 1);
            Gamemanager.Instance.HUDScript.YouDied();
            //Gamemanager.Instance.animatorFunctionsScript.PlayDieSound();
            if (!playerIsDead)
                StartCoroutine(Death());
        }
    }

    public IEnumerator Death()
    {
        playerIsDead = true;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);]

        ////play the death animaion
        //animator.SetTrigger("Dead");
        //Instantiate(particuleDead, transform.position, Quaternion.identity);
        //Gamemanager.Instance.HUDScript.YouDied();
        float timer = 1;
        while (timer > 0)
        {
            //if (!paused)
            //{
                timer -= Time.deltaTime;
            //}
            yield return null;
        }
        //yield return new WaitForSeconds(1);
        transform.position = spawnLocation;
        HP = maxHP;
        playerIsDead = false;

    }

    public void Hurt(int attackPower, int targetSide)
    {
        animator.SetTrigger("takeDamage");
        //playAnimation = true;
        Gamemanager.Instance.animatorFunctionsScript.PlayOnHitSound();
        GameObject newparticule = (GameObject)Instantiate(particuleHit, transform.position + new Vector3(Random.Range(0, 2), Random.Range(1, 3)), Quaternion.identity);
        Destroy(newparticule, 1);
        //AnimatorFunctions.Instance.PlayHitSound();
        StartCoroutine(FreezeEffect(1f, .3f));
        //animator.SetTrigger("Hurt");
        //sceneCamera.Shake(5, 1f);
        //UpdateAnimationsState("Hurt");  
        move.x = targetSide * launchPower.x;
        moveDirection.y = launchPower.y;
        HP -= attackPower;
        ////debug.log("I'm getting hurt on my side:" + targetSide + HP);

    }

    public IEnumerator FreezeEffect(float length, float timeScale)
    {
        //material.material.color = Color.white;
        move.x = 0;
        float timer = length;
        yield return new WaitForSeconds(length);
        while (timer > 0)
        {
            if (!paused)
            {
                timer -= Time.deltaTime;
            }
            yield return null;
        }
        //material.material.color = Color.blue;
        //Time.timeScale = 1;

    }

    private void UpdateAnimations()
    {
        animator.SetBool("ground", controller.isGrounded);
        animator.SetBool("grounded", controller.isGrounded);
        animator.SetFloat("velocityY", moveDirection.y);
        animator.SetFloat("velocityX", move.x);
        animator.SetBool("IsWalking", isWalking);
    }

    //=================================================/
    //State management
    //=================================================/
    internal void UpdateAnimationsState(string newState)
    {
        if (newState != currentState)
        {
            animator.Play(newState);
            currentState = newState;
        }

    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(new Vector3(controller.transform.position.x, controller.transform.position.y + 1, controller.transform.position.z), controller.transform.forward * rayRange, Color.red);
    }
    public void RespawnPosition()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            //HP = 0;
        }
    }

    void weaponColliderOn()
    {
        swordCollider.enabled = true;
        //Physics.IgnoreLayerCollision(6, 7, false);
        WeponTrail.SetActive(true);
    }

    void weaponColliderOff()
    {
        swordCollider.enabled = false;
        //Physics.IgnoreLayerCollision(6, 7, true);
        WeponTrail.SetActive(false);
    }


    void Attack()
    {
        if (Input.GetButtonDown("Fire1") && !canAttack && isAttacking)
            animator.SetTrigger("Combo");

        if (Input.GetButtonDown("Fire1") && canAttack)
        {
            //animator.SetTrigger("attack");

            AttackCombo();
            //animator.SetTrigger("attack");

            //AnimatorFunctions.Instance.PlayHitSound();
            //animator.SetTrigger("Attack");
            StartCoroutine(AtivateAttack());
            ////debug.log("Player Attack");
        }
    }

    private IEnumerator AtivateAttack()
    {

        canAttack = false;
        isAttacking = true;
        playAnimation = true;
        float timer = attackDuration;
        while (timer > 0)
        {
            if (!paused)
            {
                timer -= Time.deltaTime;
            }
            yield return null;
        }
        //yield return new WaitForSeconds(attackDuration);


        //wepon
        //GameObject weapon = Instantiate(attackWeapon, transform);
        //weapon.GetComponent<WeaponSwingScript>().spawningObject = gameObject;
        //if (raycastHit)
        //{
        //    if (hit.transform.CompareTag("Enemy") && isAttacking)
        //    {
        //        //debug.log("Player Attack,enemy hit");
        //        hit.transform.GetComponent<EnemyAI>().takeDamage(attackPower);
        //        //wepon
        //        //hit.collider.gameObject.TryGetComponent<EnemyAI>(out EnemyAI enemyScript);
        //        //weapon.GetComponent<WeaponSwingScript>().playerHittingEnemyScript = enemyScript;

        //    }
        //}
        //GameObject.TryGetComponent<EnemyAI>(out EnemyAI enemyScript);
        //weapon.GetComponent<WeaponSwingScript>().playerHittingEnemyScript = enemyScript;
        isAttacking = false;
        canAttack = true;
    }

    void AttackCombo()
    {
        if (comboStep == 0)
        {
            if (haveSword)
            {
                animator.Play("WGS_attackA1");
                playAnimationSpeed = 6;
                GameObject newparticule = (GameObject)Instantiate(particuleAttack, transform.position + new Vector3(Random.Range(-2, 2), Random.Range(0, 3)), Quaternion.identity);
                Destroy(newparticule, 1);
            }
            else
            {
                //animator.Play("Attack");
                animator.SetTrigger("attack");
            }

            comboStep = 1;
            return;
        }
        if (comboStep != 0)
        {
            if (comboPossible)
            {
                comboPossible = false;
                comboStep += 1;
            }
        }
    }

    public void ComboPossible()
    {
        comboPossible = true;
    }

    public void Combo()
    {

        if (haveSword)
        {
            if (comboStep == 2)
            {
                animator.Play("WGS_attackA2");
                playAnimationSpeed = 6;
            }

            if (comboStep == 3)
            {
                animator.Play("WGS_attackA3");
                playAnimationSpeed = 12;
            }
            if (comboStep == 4)
            {
                animator.Play("WGS_attackA4");
                playAnimationSpeed = -6;
            }
            //if (comboStep == 5)
            //{
            //    animator.Play("WGS_attackA5");
            //    playAnimationSpeed = 6;
            //}
        }
        //else
        //{
        //    if (comboStep == 2)
        //     animator.Play("attackA_2of2");
        //    //animator.SetTrigger("attack");
        //}

    }
    public void ComboReset()
    {
        comboPossible = false;
        comboStep = 0;
        playAnimation = false;
    }
    
    
    private IEnumerator DashCoroutine()
    {
        Gamemanager.Instance.animatorFunctionsScript.PlayDashSound();
        float startTime = Time.time; // need to remember this to know how long to dash
        while (Time.time < startTime + dashTime)
        {
            if (!paused)
            {
                //move.y = Input.GetAxis("Horizontal") * playerSpeed;
                //controller.Move(move * dashSpeed * Time.deltaTime);
                //controller.Move((move + (Vector3)pushBack + moveDirection) * dashSpeed * Time.deltaTime);
                //move.x = Input.GetAxis("Horizontal") * dashSpeed * Time.deltaTime;
                //moveDirection.x *= dashSpeed;
                //moveDirection.y *= dashSpeed;
                isDshing = true;
                DashTrail.SetActive(true);
            }
            else {
                startTime += Time.deltaTime;
            }
            yield return null; // this will make Unity stop here and continue next frame
            
        }
        isDshing = false;
       // DashTrail.SetActive(false);
    }
    public void PlayStepSound()
    {
        //AnimatorFunctions.Instance.PlayStepSound();
        Gamemanager.Instance.animatorFunctionsScript.PlayStepSound();
        //Gamemanager.Instance.animatorFunctionsScript.PlayOnHitSound();
    }
    public void PlayAttackSound()
    {
        
        Gamemanager.Instance.animatorFunctionsScript.PlayHitSound();
        Gamemanager.Instance.animatorFunctionsScript.PlayHitEmitParticles();
    }
    public void PlayOnHit()
    {
       
        Gamemanager.Instance.animatorFunctionsScript.PlayOnHitSound();
        Gamemanager.Instance.animatorFunctionsScript.PlayOnHitEmitParticles();
    }

    public void PlayDustEmit()
    {
        //AnimatorFunctions.Instance.PlayStepEmitParticles();
        Gamemanager.Instance.animatorFunctionsScript.PlayStepEmitParticles();
    }

    public void PlayWeponEmit()
    {
        //AnimatorFunctions.Instance.PlayStepEmitParticles();
        Gamemanager.Instance.animatorFunctionsScript.PlayStepEmitParticles();
    }

    public void DrawSword()
    {
        drawSword.SetActive(true);
        unDrawSword.SetActive(false);
    }
    public void UnDrawSword()
    {
        drawSword.SetActive(false);
        unDrawSword.SetActive(true);
    }
    public void Pause() {
        paused = !paused;

        if (paused)
        {
            animator.enabled = false;
        }
        else {
            animator.enabled = true;
        }
    }
    private void OnDestroy()
    {
        HUDScript.pauseGame -= Pause;
    }
}
