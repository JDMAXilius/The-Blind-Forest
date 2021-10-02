using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleEnemy : MonoBehaviour
{
    EnemyAI enemyScript;
    Rigidbody rb;
    [SerializeField] GameObject face;
    [SerializeField] GameObject model;
    [SerializeField] GameObject parent;
    [SerializeField] GameObject chaseDetection;
    [HideInInspector] public Animator modelAnim;
    float lookAroundTimer;
    bool isWaiting;

    public float basicSpeed;
    float speed;
    public bool faceLeft = true;
    bool prevFacing;
    private int targetSide;
    public bool paused;

    #region Attack Variables
    [Space]
    [Header("Attack properties")]
    public float attackDelay = 3;
    float attackTimer = 0;
    public int damage = 25;
    public float attackDistance = 2;
    public float attackAngle = 90;

    [HideInInspector] public bool canAttack = true;

    [Space]
    public float afterAttackDelay;
    float afterAttackTimer = 0;
    float attackingDelay = .15f;//Time to sync with animation
    float runningattackingTimer;
    bool hasAttacked = true;

    [Space]
    [Tooltip("Amount of time to react to changing direction")]
    public float chaseDelay;
    [SerializeField] float chasingSpeed;
    [HideInInspector] public float chaseTimer;
    [HideInInspector] public bool chasePlayer = false;
    #endregion
    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody>();
        enemyScript = GetComponent<EnemyAI>();
        enemyScript.parent = parent;
        modelAnim = model.GetComponent<Animator>();
        if (faceLeft)
        {
            rb.velocity = Vector2.left * speed;
            targetSide = -1;
        }
        else
        {
            rb.velocity = Vector2.right * speed;
            targetSide = 1;
        }
        lookAroundTimer = Random.Range(10f, 20f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {
            if (chasePlayer)
            {
                if (prevFacing != Gamemanager.Instance.player.transform.position.x < transform.position.x)
                {
                    //debug.log("faceLeft != prevFacing");
                    chaseTimer = chaseDelay;
                }
                else
                {
                    if (chaseTimer <= 0)
                    {
                        faceLeft = Gamemanager.Instance.player.transform.position.x < transform.position.x ? true : false;
                    }
                    chaseTimer -= Time.deltaTime;
                }
                speed = chasingSpeed;
                isWaiting = false;
            }
            else
            {
                speed = basicSpeed;
            }
            if (enemyScript.afterDamageTimer <= 0 && afterAttackTimer <= 0 && !isWaiting)
            {
                rb.velocity = (faceLeft ? Vector3.left : Vector3.right) * speed;
            }

            if (lookAroundTimer <= 0)
            {
                if (!chasePlayer && enemyScript.afterDamageTimer <= 0 && afterAttackTimer <= 0 && !chasePlayer)
                {
                    rb.velocity = Vector3.zero;
                    int trigger = Random.Range(1, 3);
                    if (trigger == 1)
                    {
                        modelAnim.SetTrigger("Look");
                    }
                    else
                    {
                        modelAnim.SetTrigger("Scream");
                    }
                    lookAroundTimer = Random.Range(10f, 20f);
                }
            }
            else
            {
                lookAroundTimer -= Time.deltaTime;
            }

            if (faceLeft)
            {
                face.transform.localPosition = (Vector3.left + Vector3.up) * .5f;
                model.transform.localRotation = Quaternion.Euler(Vector3.down * 90);
                model.transform.localPosition = Vector3.right * .04f + Vector3.down;
                chaseDetection.transform.localPosition = Vector3.left * 1.25f;
            }
            else
            {
                face.transform.localPosition = (Vector3.right + Vector3.up) * .5f;
                model.transform.localRotation = Quaternion.Euler(Vector3.up * 90);
                model.transform.localPosition = Vector3.left * .04f + Vector3.down;
                chaseDetection.transform.localPosition = Vector3.right * 1.25f;
            }
            Attack();
            AfterAttack();
            ChooseAnimation();
            prevFacing = Gamemanager.Instance.player.transform.position.x < transform.position.x;
        }
    }

    public void SwapDirection()
    {
        rb.velocity = new Vector2(rb.velocity.x * -1, rb.velocity.y);
        faceLeft = !faceLeft;
        face.transform.localPosition = new Vector3(face.transform.localPosition.x * -1, face.transform.localPosition.y, face.transform.localPosition.z);
        targetSide *= -1;
    }
    #region Attack
    void Attack()
    {
        float y = Mathf.Tan(Mathf.Deg2Rad * (attackAngle / 2));
        Debug.DrawLine(transform.position, ((faceLeft ? Vector3.left : Vector3.right) + (Vector3.up * y)).normalized * attackDistance + transform.position, Color.red);
        Debug.DrawLine(transform.position, ((faceLeft ? Vector3.left : Vector3.right) + (Vector3.down * y)).normalized * attackDistance + transform.position, Color.red);
        Debug.DrawRay(transform.position, (faceLeft ? Vector3.left : Vector3.right) * attackDistance, Color.red);

        if (Vector3.Distance(transform.position, Gamemanager.Instance.player.transform.position) <= attackDistance)
        {
            Vector3 targetDir = Gamemanager.Instance.player.transform.position - transform.position;

            float angle = Vector3.Angle(targetDir, (faceLeft ? Vector3.left : Vector3.right) * attackDistance);
            Debug.DrawRay(transform.position, targetDir, Color.red);
            if (Physics.Raycast(transform.position, targetDir, out RaycastHit r, attackDistance))
            {
                if (angle <= attackAngle && r.collider.gameObject.CompareTag("Player"))
                {
                    rb.velocity = Vector3.zero;
                    if (canAttack && attackTimer <= 0)
                    {
                        chasePlayer = true;
                        modelAnim.SetTrigger("Attack");
                        Attacking();
                        attackTimer = attackDelay;
                    }
                }
            }
            attackTimer -= Time.deltaTime;
        }
    }
    void Attacking()
    {
        canAttack = false;
        rb.velocity = Vector3.zero;
        afterAttackTimer = afterAttackDelay;
        runningattackingTimer = attackingDelay;
        hasAttacked = false;
        //Gamemanager.Instance.playerScript.Hurt(damParam, targetSide);//this the fuction for the player get damage
    }
    public void HitPlayer()
    {
        Vector3 targetDir = Gamemanager.Instance.player.transform.position - transform.position;

        float angle = Vector3.Angle(targetDir, (faceLeft ? Vector3.left : Vector3.right) * attackDistance);
        if (Physics.Raycast(transform.position, targetDir, out RaycastHit r, attackDistance))
        {
            if (angle <= attackAngle && r.collider.gameObject.CompareTag("Player"))
            {
                Gamemanager.Instance.playerScript.Hurt(damage, targetSide);//this the fuction for the player get damage
            }
        }
    }
    void AfterAttack()
    {
        if (runningattackingTimer > 0)
        {
            runningattackingTimer -= Time.deltaTime;
        }
        else
        {
            if (!hasAttacked)
            {
                HitPlayer();
                hasAttacked = true;
            }
        }

        if (afterAttackTimer > 0)
            afterAttackTimer -= Time.deltaTime;
        else
        {
            canAttack = true;
        }
    }
    void ChooseAnimation()
    {
        if (!modelAnim.GetCurrentAnimatorStateInfo(0).IsName("Look") && !modelAnim.GetCurrentAnimatorStateInfo(0).IsName("Scream"))
        {
            isWaiting = false;
        }
        else
        {
            isWaiting = true;
        }
        float absVel = Mathf.Abs(rb.velocity.x);
        if (absVel != 0)
        {
            if (absVel == basicSpeed)
            {
                modelAnim.SetTrigger("Walk");
            }
            else
            {
                modelAnim.SetTrigger("Run");
            }
        }
        else
        {
            if (!isWaiting)
            {
                modelAnim.SetTrigger("Idle");
            }
        }
    }
    #endregion
    public void Pause(int pauseOverride = 2)
    {
        if (pauseOverride == 2)
        {
            paused = !paused;
        }
        else if (pauseOverride == 1)
        {
            paused = true;
        }
        else if (pauseOverride == 0)
        {
            paused = false;
        }
        if (paused)
        {
            modelAnim.enabled = false;
        }
        else
        {
            modelAnim.enabled = true;
        }
    }
}
