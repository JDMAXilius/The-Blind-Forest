using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    [HideInInspector] public GameObject parent;
    [HideInInspector]public Transform playerPos;
    [HideInInspector]public Vector2 direction = Vector2.zero;

    [Header("Bullet Propeties")]
    public float speed;
    public int damage;
    public bool isTracking;
    public float lifeTime = 2f;

    Rigidbody rb;
    SphereCollider sphere;

    public bool paused;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sphere = GetComponent<SphereCollider>();
        //Setting direction. If direction == (0,0), instantiating object did not give a direction so it defaults to the player
        //Otherwise, it uses the direction given to it(i.e fixed spider)
        if (direction == Vector2.zero)
        {
            direction = new Vector2(playerPos.position.x - transform.position.x, playerPos.position.y - transform.position.y);
        }

        direction.Normalize();

        rb.velocity = direction * speed;

        HUDScript.pauseGame += Pause;
    }

    void Update()
    {
        if (!paused)
        {
            //Destroy itself after an amount of time
            if (lifeTime > 0)
            {
                lifeTime -= Time.deltaTime;
                if (isTracking && playerPos)
                {
                    direction = new Vector2(playerPos.position.x - transform.position.x, playerPos.position.y - transform.position.y);
                    direction.Normalize();
                    rb.velocity = direction * speed;
                }
                Debug.DrawRay(transform.position, direction * (speed / 50), Color.red);
                if (speed / 50 > .35f)
                {
                    if (Physics.SphereCast(transform.position, sphere.radius, direction * (speed / 50), out RaycastHit hit, rb.velocity.magnitude / 50))
                    {
                        transform.position = hit.point;
                    }
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
        

    }

    void OnTriggerEnter(Collider collision)
    {
        ////debug.log(collision.gameObject.name);
        if (collision.gameObject == Gamemanager.Instance.player)
        //if (collision.gameObject.tag == "Player") 
        {
            int targetSide;
            if (transform.position.x < Gamemanager.Instance.player.transform.position.x)
            {
                targetSide = 1;
            }
            else
            {
                targetSide = -1;
            }
            Gamemanager.Instance.playerScript.Hurt(damage, targetSide);
        }
        if (collision.gameObject.name.Contains("DestructableBox")) { 
            if(collision.gameObject.TryGetComponent<EnemyAI>(out EnemyAI e)) {
                e.takeDamage(2);
            }
        }
        Destroy(parent);
        Destroy(gameObject);

    }
    private void OnDestroy()
    {
        HUDScript.pauseGame -= Pause;
    }
    public void Pause() {
        
        paused = !paused;

        if (paused)
        {
            rb.velocity = Vector3.zero;
        }
        else
        {
            rb.velocity = direction * speed;
        }
    }
}

