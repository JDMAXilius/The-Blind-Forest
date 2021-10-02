using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    bool canShoot = true;
    [SerializeField] float shootTimer;
    float runningTimer;

    float hor;
    float prevHor;
    float vert;
    Vector2 direction;

    [SerializeField] int damage;
    [SerializeField] float maxDistance;
    [SerializeField] int manaThreshold = 25;
    [SerializeField] float laserWidth = .1f;
    [SerializeField] float laserLifeSpan;
    [SerializeField] GameObject laserArm;
    float runningLifetime;
    LineRenderer laser;
    [SerializeField] Material aimingMaterial;
    bool aimingMat;
    [SerializeField] Material laserMaterial;
    bool paused;

    AudioSource laserAudio;
    public GameObject particule;
    // Start is called before the first frame update
    void Start()
    {
        laser = GetComponent<LineRenderer>();
        laser.startWidth = laserWidth;
        HUDScript.pauseGame += Pause;
        laserAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {
            hor = Input.GetAxis("Horizontal");
            if (hor == 0)
            {
                hor = prevHor;
            }
            prevHor = hor;
            vert = Input.GetAxis("Vertical");

            direction = ((Vector2.right * hor) + (Vector2.up * vert));
            direction.Normalize();
            direction *= (maxDistance != 0 ? maxDistance : 1000);
            laserArm.transform.right = (Vector3)direction - transform.position;

            if (canShoot && Input.GetAxis("LaserHold") < 0)
            {
                laser.startWidth = .1f;
                laser.material = aimingMaterial;
                aimingMat = true;
                laser.SetPosition(0, transform.position);
                if (Physics.SphereCast(transform.position, laserWidth, direction, out RaycastHit hit, maxDistance != 0 ? maxDistance : 1000))
                {
                    laser.SetPosition(1, hit.point);
                }
                else
                {
                    ////debug.log("Max " + maxDistance);
                    laser.SetPosition(1, transform.position + ((Vector3)direction.normalized * (maxDistance != 0 ? maxDistance : 1000)));
                }
            }
            if (!(Input.GetAxis("LaserHold") < 0) || !canShoot)
            {
                if (aimingMat)
                {
                    laser.SetPosition(0, Vector3.zero);
                    laser.SetPosition(1, Vector3.zero);
                }
            }

            if (Input.GetAxis("Lasser Attack") == 1 && canShoot)
            {
                Shoot();
                   
            }

            if (runningTimer > 0)
            {
                runningTimer -= Time.deltaTime;
            }
            else
            {
                canShoot = true;
            }
        }

    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, direction, Color.cyan);
    }
    void Shoot()
    {
        if (Gamemanager.Instance.playerScript.Mana >= manaThreshold)
        {
            Gamemanager.Instance.playerScript.Mana -= manaThreshold;

            laserAudio.Play();
            GameObject newparticule = (GameObject)Instantiate(particule, transform.position, Quaternion.identity);
            Destroy(newparticule, 1);

            runningTimer = shootTimer;
            canShoot = false;
            aimingMat = false;
            laser.startWidth = laserWidth;
            laser.material = laserMaterial;
            laser.SetPosition(0, transform.position);
            if (Physics.SphereCast(transform.position, laserWidth, direction, out RaycastHit hit, maxDistance != 0 ? maxDistance : 1000))
            {
                laser.SetPosition(1, hit.point);
                if (hit.collider.gameObject.TryGetComponent<EnemyAI>(out EnemyAI enemyScript))
                {
                    enemyScript.takeDamage(damage, true);
                }
            }
            else
            {
                //laser.SetPosition(1, transform.position + ((Vector3.right * (maxDistance != 0 ? maxDistance : 1000) * hor/Mathf.Abs(hor)) + (Vector3.up * (maxDistance != 0 ? maxDistance : 1000) * hor / Mathf.Abs(vert))));
                laser.SetPosition(1, transform.position + ((Vector3)direction.normalized * (maxDistance != 0 ? maxDistance : 1000)));
            }
            StartCoroutine(LineDissipate());
        }
    }

    IEnumerator LineDissipate()
    {
        runningLifetime = laserLifeSpan;
        while (laser.startWidth > 0)
        {
            if (!paused)
            {
                laser.startWidth = laserWidth * ((runningLifetime -= Time.deltaTime) / laserLifeSpan);
                //laser.startWidth -= .001f;
            }
            yield return null;
        }
        laser.SetPosition(0, Vector3.zero);
        laser.SetPosition(1, Vector3.zero);
        laser.startWidth = laserWidth;
    }

    private void OnDestroy()
    {
        HUDScript.pauseGame -= Pause;
    }
    public void Pause()
    {
        paused = !paused;
    }
}

