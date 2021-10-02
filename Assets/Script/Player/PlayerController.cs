using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
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
    public MeshRenderer material;
    [SerializeField]
    public SceneCamera sceneCamera;
    bool isWalking = false;
    bool isGrounded;

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
    bool paused;

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
    public int attackPower = 25;
    private bool isAttacking = false;
    private bool canAttack = true;
    public int attackRange = 3;
    [SerializeField] GameObject attackWeapon;
    public float weaponStartRotation = 15;

    public Vector3 spawnLocation;
    public bool playerIsDead = false;

    public Collider swordCollider;
    
    //WallJump Variables
    //public float wallMultiplier = 0.75F;
    //public float wallJumpSpeed = 6.0F;

    //Animator Functions
    public AudioSource sfxAudioSource;
    public ParticleSystem ParticuleSource;
    //public GameObject animatorFunctions;
   
    void Start()
    {
        //Physics.IgnoreLayerCollision(6, 7, true);
        maxHP = HP;
        maxMana = Mana;
        material = GetComponent<MeshRenderer>();
        material.material.color = Color.blue;
        controller = gameObject.GetComponent<CharacterController>();
        //controller = gameObject.AddComponent<CharacterController>();
       
        //drawSword.SetActive(false);
        //unDrawSword.SetActive(true);

        //animatorFunction = Gamemanager.Instance.animatorFunctions;

        spawnLocation = GameObject.FindGameObjectWithTag("Respawn").transform.position;
        transform.position = spawnLocation;

        //if (haveCheckPoint)
        //{
        //    transform.position = Gamemanager.Instance.lastCheckPointPos;
        //    //debug.log("truehaveCheckPoint");
        //    //debug.log(Gamemanager.Instance.lastCheckPointPos);
        //    //debug.log(transform.position);
        //}
        //else
        //{

        //    transform.position = GameObject.FindGameObjectWithTag("Respawn").transform.position;
        //    //debug.log("falsehaveCheckPoint");
        //    //debug.log(Gamemanager.Instance.lastCheckPointPos);
        //    //debug.log(transform.position);
        //}
        HUDScript.pauseGame += Pause;
    }
    void Update()
    {
        if (!paused)
        {
            if (HP > maxHP)
            {
                HP = maxHP;
            }
            if (Mana > maxMana) {
                Mana = maxMana;
            }
            //transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            transform.position.Set(transform.position.x, transform.position.y, 0);
            if (!playerIsDead)
            {

                isGrounded = controller.isGrounded;
                //Gamemanager.Instance.playerScript.ParticuleSource = AnimatorFunctions.Instance.playStepEmitParticles;
                pushBack = Vector2.Lerp(pushBack, Vector2.zero, 6 * Time.deltaTime);

                //Raycast hits bools
                raycastHit = Physics.Raycast(transform.position, transform.forward, out hit, rayRange);

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
                //if (move == Vector3.zero)
                //{
                //    rotation = new Vector3(0,180,0);
                //    gameObject.transform. = new Vector3(0, 180, 0);
                //}
                // Dash();
                if (controller.isGrounded)
                {
                    move.x = Input.GetAxis("Horizontal") * playerSpeed;
                    //controller.Move(move * Time.deltaTime * playerSpeed);
                    //moveDirection.x = Input.GetAxis("Horizontal") * playerSpeed;
                    right = false;
                    left = true;
                    //isWalking = false;
                    coyoteCounter = coyoteTime;
                    dJumpCounter = 0;
                    doubleJump = false;
                }
                else if (!controller.isGrounded)
                {
                    if (move.x >= 0)
                    {
                        move.x = Input.GetAxis("Horizontal") * airspeed;

                        ////debug.log("right");
                        right = true;
                        //isWalking = true;
                        if (left)
                        {
                            //move.x = Input.GetAxis("Horizontal") * airspeed / 2;
                            //controller.Move((move * Time.deltaTime * airspeed)/2);
                            ////debug.log("Change left");
                            //moveDirection.x = Input.GetAxis("Horizontal") * airspeed / 2;
                            right = false;

                        }

                    }
                    else if (move.x <= 0)
                    {
                        move.x = Input.GetAxis("Horizontal") * airspeed;
                        ////debug.log("left");
                        left = true;
                        //isWalking = true;
                        if (right)
                        {
                            //move.x = Input.GetAxis("Horizontal") * airspeed / 2;
                            //controller.Move((move * Time.deltaTime * airspeed)/2);
                            ////debug.log("Change right");
                            //moveDirection.x = Input.GetAxis("Horizontal") * airspeed / 2;
                            left = false;
                        }

                    }

                    coyoteCounter -= Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    move.x = 0;
                }
                controller.Move((move + (Vector3)pushBack) * Time.deltaTime);

                if (Input.GetButtonDown("Jump"))
                {

                    if (isGrounded && coyoteCounter > 0 && dJumpCounter == 0)
                    {
                        animator.SetTrigger("jump");
                        Gamemanager.Instance.animatorFunctionsScript.PlayJumpSound();
                        Gamemanager.Instance.animatorFunctionsScript.PlayStepEmitParticles();
                        moveDirection.y = jumpHeight;
                        dJumpCounter = 0;
                        controller.transform.position = new Vector3(transform.position.x, transform.position.y + 10, 0);
                    }
                    if (!isGrounded && dJumpCounter < nrOfAlowedDJumps)
                    {

                        animator.SetTrigger("doubleJump");
                        Gamemanager.Instance.animatorFunctionsScript.PlayJumpSound();
                        Gamemanager.Instance.animatorFunctionsScript.PlayStepEmitParticles();
                        moveDirection.y = jumpHeight;
                        dJumpCounter++;
                    }
                }



                //player fall or gravity
                moveDirection.y -= gravityValue * Time.deltaTime;

                //movement of the player
                controller.Move(moveDirection * Time.deltaTime);

                //fix stuck above
                if ((controller.collisionFlags & CollisionFlags.Above) != 0)
                {
                    moveDirection.y -= 1;
                }

                if (controller.isGrounded && (controller.collisionFlags & CollisionFlags.Below) != 0)
                {
                    if (move.x != 0)
                    {
                        isWalking = true;
                    }
                    else
                    {
                        isWalking = false;
                    }
                }

                if (Input.GetButtonDown("Dash"))
                {
                    animator.SetTrigger("dash");
                    StartCoroutine(DashCoroutine());
                }
                if (Input.GetButtonDown("DrawSword"))
                {
                    if (!haveSword)
                    {
                        haveSword = true;
                        //debug.log("haveSword = true");
                    }
                    else
                    {
                        //debug.log("haveSword = false");
                        haveSword = false;
                    }
                    animator.SetBool("haveSword", haveSword);
                    animator.SetTrigger("DrawSword");

                }

                //function
                Health();
                //UpdateAnimations();
                RespawnPosition();
                Attack();
            }
        }
    }

    private void Health()
    {
        //If health < 0, destroy me
        if (HP <= 0)
        {
            animator.SetTrigger("Dead");
            ///Gamemanager.Instance.HUDScript.YouDied();
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
        Gamemanager.Instance.HUDScript.YouDied();
        yield return new WaitForSeconds(1);
        transform.position = spawnLocation;
        HP = maxHP;
        playerIsDead = false;
        
    }

    public void Hurt(int attackPower, int targetSide)
    {
        animator.SetTrigger("takeDamage");
        Gamemanager.Instance.animatorFunctionsScript.PlayOnHitSound();
        //AnimatorFunctions.Instance.PlayHitSound();
        StartCoroutine(FreezeEffect(.1f, .3f));
        //animator.SetTrigger("Hurt");
        sceneCamera.Shake(5, 1f);
        //UpdateAnimationsState("Hurt");  
        move.x = targetSide * launchPower.x;
        moveDirection.y = launchPower.y;
        HP -= attackPower;
        ////debug.log("I'm getting hurt on my side:" + targetSide + HP);

    }

    public IEnumerator FreezeEffect(float length, float timeScale)
    {
        material.material.color = Color.white;
        //Time.timeScale = timeScale;
        yield return new WaitForSeconds(length);
        material.material.color = Color.blue;
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
        Debug.DrawRay(new Vector3(controller.transform.position.x, controller.transform.position.y+1, controller.transform.position.z ), controller.transform.forward * rayRange, Color.red);
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
    }

    void weaponColliderOff()
    {
        swordCollider.enabled = false;
        //Physics.IgnoreLayerCollision(6, 7, true);
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
        yield return new WaitForSeconds(attackDuration);


        //wepon
        GameObject weapon = Instantiate(attackWeapon, transform);
        weapon.GetComponent<WeaponSwingScript>().spawningObject = gameObject;
        if (raycastHit)
        {
            if (hit.transform.CompareTag("Enemy") && isAttacking)
            {
                //debug.log("Player Attack,enemy hit");
                hit.transform.GetComponent<EnemyAI>().takeDamage(attackPower);
                //wepon
                hit.collider.gameObject.TryGetComponent<EnemyAI>(out EnemyAI enemyScript);
                weapon.GetComponent<WeaponSwingScript>().playerHittingEnemyScript = enemyScript;

            }
        }
       // GameObject.TryGetComponent<EnemyAI>(out EnemyAI enemyScript);
        //weapon.GetComponent<WeaponSwingScript>().playerHittingEnemyScript = enemyScript;
        isAttacking = false;
        canAttack = true;
    }

    void AttackCombo()
    {
        if (comboStep ==0)
        {
            if (haveSword)
            {
                animator.Play("WGS_attackA1");
            }
            else
            {
                //animator.Play("Attack");
                animator.SetTrigger("attack");
            }

            comboStep = 1;
            return;
        }
        if (comboStep !=0)
        {
            if (comboPossible)
            {
                comboPossible = false;
                comboStep +=1;
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
            }

            if (comboStep == 3)
            {
                animator.Play("WGS_attackA3");
            }
            if (comboStep == 4)
            {
                animator.Play("WGS_attackA4");
            }
            if (comboStep == 5)
            {
                animator.Play("WGS_attackA5");
            }
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
    }

    ////WallJump
    //private void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    if (hit.normal.y < 0.1f && !controller.isGrounded)
    //    {
    //        currentWallMultiplier = wallMultiplier;
    //        dJumpCounter = 0;
    //        wallCollision = true;
    //        if (Input.GetButtonDown("Jump"))
    //        {
    //            //playerMovement.moveDirection.x = Input.GetAxis("Horizontal") * wallJumpSpeed;
    //            moveDirection = hit.normal * wallJumpSpeed;
    //            moveDirection.y = jumpHeight;
    //            controller.Move(moveDirection * Time.deltaTime);
    //            print("walljump");
    //        }
    //    }
    //    else
    //    {
    //        currentWallMultiplier = 1.0F;
    //        wallCollision = false;
    //    }
    //}
    private IEnumerator DashCoroutine()
    {
        Gamemanager.Instance.animatorFunctionsScript.PlayDashSound();
        float startTime = Time.time; // need to remember this to know how long to dash
        while (Time.time < startTime + dashTime)
        {
            controller.Move(move * dashSpeed * Time.deltaTime);
            
            yield return null; // this will make Unity stop here and continue next frame
        }
    }
    public void PlayStepSound()
    {
        //AnimatorFunctions.Instance.PlayStepSound();
        Gamemanager.Instance.animatorFunctionsScript.PlayStepSound();
    }
    public void PlayAttackSound()
    {
        //AnimatorFunctions.Instance.PlayStepSound();
        Gamemanager.Instance.animatorFunctionsScript.PlayHitSound();
    }
    
    public void PlayDustEmit()
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
    void Pause()
    {
        paused = !paused;
    }
}



