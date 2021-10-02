using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ShootScript : MonoBehaviour
{
    public bool paused;

    [Header("Shooting Properties")]
    [SerializeField] float shootTimer;
     public float runningTimer;
    //Use wiggleShots to increase the spread of the projectiles. No negative numbers, they will be converted to zero 
    [SerializeField] float wiggleShots;
    public bool canShoot = true;

    [Space]
    [Header("Object references for the prefab")]
    [SerializeField] GameObject projectile;
    [SerializeField] GameObject bulletSpawn;
    Transform playerPos;
    [Space]
    [Header("Bullet Properties")]
    public float projectileLifeTime;
    public bool trackingBullets;
    public float bulletSpeed;
    public int bulletDamage;
    [SerializeField] Vector3 pivotAdjust = new Vector3(0, 1, 0);
    //Divide a speed by 50 to get its translation in units

    TurretEnemy turretEnemyCheck;
    FlyingEnemy flyingEnemyCheck;
    Vector3 directionToFace = Vector3.zero;

    [SerializeField] AudioSource shootSource;
    [SerializeField] AudioClip shootSound;
    // Start is called before the first frame update
    void Start()
    {
        playerPos = Gamemanager.Instance.player.transform;
        //Set active timer
        //runningTimer = shootTimer;

        if (!TryGetComponent<EnemyAI>(out EnemyAI e))
        {
            HUDScript.pauseGame += Pause;
        }

        if (TryGetComponent<TurretEnemy>(out TurretEnemy t))
        {
            turretEnemyCheck = t;
            directionToFace = transform.up;
        }
        else if (TryGetComponent<FlyingEnemy>(out FlyingEnemy f))
        {
            flyingEnemyCheck = f;
            if (f.shootTowardsPlayer)
            {
                directionToFace = transform.position - playerPos.position + pivotAdjust;
            }
            else
            {
                directionToFace = transform.forward;
            }

        }
        else {
            directionToFace = transform.up;
        }
        //Negative wiggle value check
        if (wiggleShots < 0)
        {
            wiggleShots = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {
            //Shooting timer
            if (runningTimer <= 0 && canShoot)
            {
                Shoot();
            }

            //Time passed
            runningTimer -= Time.deltaTime;


            if (turretEnemyCheck)
            {
                directionToFace = transform.up;
            }
            else if (flyingEnemyCheck)
            {
                if (flyingEnemyCheck.shootTowardsPlayer)
                {
                    directionToFace = transform.position - playerPos.position - pivotAdjust;
                }
                else
                {
                    directionToFace = transform.forward - flyingEnemyCheck.rb.velocity;
                }
            }
            else {
                directionToFace = transform.up;
            }

            Vector3 bulletDistance = Vector3.Normalize(directionToFace);
            bulletDistance *= (bulletSpeed * projectileLifeTime);
            Debug.DrawRay(bulletSpawn.transform.position, -bulletDistance, Color.red);
        }
    }
    void Shoot()
    {
        if (flyingEnemyCheck)
            flyingEnemyCheck.modelAnim.SetTrigger("Attack02");
        GameObject projClone = Instantiate(projectile);
        projClone.transform.position = bulletSpawn.transform.position;
        projClone.GetComponent<ProjectileScript>().playerPos = playerPos;
        projClone.GetComponent<ProjectileScript>().isTracking = trackingBullets;
        //projClone.GetComponent<ProjectileScript>().paused = paused;
        if (bulletDamage != 0)
        {
            projClone.GetComponent<ProjectileScript>().damage = bulletDamage;
        }
        if (bulletSpeed != 0)
        {
            projClone.GetComponent<ProjectileScript>().speed = bulletSpeed;
        }
        if (projectileLifeTime > 0)
        {
            projClone.GetComponent<ProjectileScript>().lifeTime = projectileLifeTime;
        }
        //Direction projectile will move
        Vector2 tempDirection = directionToFace;
        //Apply wiggle, 0 applies no wiggle
        tempDirection.Set(tempDirection.x + Random.Range(-wiggleShots, wiggleShots), tempDirection.y + Random.Range(-wiggleShots, wiggleShots));
        //Sets new projectile direction
        projClone.GetComponent<ProjectileScript>().direction = -tempDirection;

        //Reset timer
        runningTimer = shootTimer;

        if (shootSource && shootSound) {
            shootSource.PlayOneShot(shootSound,.05f);
        }
    }
    private void OnDestroy()
    {
        if (!TryGetComponent<EnemyAI>(out EnemyAI e))
        {
            HUDScript.pauseGame -= Pause;
        }
    }
    public void Pause()
    {
        ////debug.log("Paused " + gameObject.GetInstanceID());
        paused = !paused;
    }
}
