using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwingScript : MonoBehaviour
{

    /* Instantiating the weapon - Copy this and adjust if necessary. passed values are for the player's hurt function
        GameObject weapon = Instantiate(attackWeapon, transform);
        weapon.GetComponent<WeaponSwingScript>().spawningObject = gameObject;
        weapon.GetComponent<WeaponSwingScript>().passedDamage = damParam;
        weapon.GetComponent<WeaponSwingScript>().passedTargetSide = targetSide;

        GameObject weapon = Instantiate(attackWeapon, transform);
        weapon.GetComponent<WeaponSwingScript>().spawningObject = gameObject;
        hit.collider.gameObject.TryGetComponent<EnemyAI>(out EnemyAI enemyScript);
        weapon.GetComponent<WeaponSwingScript>().playerHittingEnemyScript = enemyScript;
    */

    [SerializeField] float rotateBy;
    [SerializeField] float rotateSpeed;
    [SerializeField] Transform rod;
    [SerializeField] Transform top;
    public GameObject spawningObject;
    //If the player hits an enemy, get the reference to the enemy
    public EnemyAI playerHittingEnemyScript;

    float rotatingDegrees;

    public bool faceLeft = true;
    public float initRot = 15;
    public float attackDistance = 2;
    public int passedDamage;
    public int passedTargetSide;

    bool hasDamaged = false;
    bool canAttack;
    // Start is called before the first frame update
    void Start()
    {
        if (spawningObject.TryGetComponent<PatrolEnemy>(out PatrolEnemy patrol))
        {
            attackDistance = patrol.attackDistance;
            faceLeft = patrol.faceLeft;
            //initRot = patrol.weaponStartRotation;
        }
        else if (spawningObject.TryGetComponent<PlayerController>(out PlayerController player)) {
            attackDistance = player.attackRange;
            initRot = player.weaponStartRotation;
            faceLeft = player.left;
            passedDamage = player.attackPower;
            transform.Rotate(Vector3.up * 90);
        }
        rotatingDegrees = rotateBy;
        transform.Rotate(Vector3.forward * (faceLeft ? initRot : -initRot));
        top.localPosition = Vector3.up * attackDistance;
        rod.localPosition = Vector3.up * (attackDistance/2);
        rod.localScale = new Vector3(rod.localScale.x, attackDistance / 2, rod.localScale.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (spawningObject)
        {
            if (rotatingDegrees > 0)
            {
                if (faceLeft)
                {
                    transform.Rotate(Vector3.forward, rotateSpeed);
                }
                else
                {
                    transform.Rotate(Vector3.forward, -rotateSpeed);
                }
                rotatingDegrees -= rotateSpeed;

                if (rotatingDegrees <= rotateBy / 2 && !hasDamaged)
                {
                    hasDamaged = true;
                    if (spawningObject.TryGetComponent<PatrolEnemy>(out PatrolEnemy patrol))
                    {
                        Gamemanager.Instance.playerScript.Hurt(passedDamage, passedTargetSide);
                    }
                    else if (spawningObject.TryGetComponent<PlayerController>(out PlayerController player))
                    {
                       // playerHittingEnemyScript.takeDamage(passedDamage);
                        if (playerHittingEnemyScript)
                        {
                            playerHittingEnemyScript.takeDamage(passedDamage);
                        }
                    }

                    //else if(!spawningObject.TryGetComponent<PlayerController>(out PlayerController playernot))
                    //{
                    //    playerHittingEnemyScript.takeDamage(0);
                    //}
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else {
            Destroy(gameObject);
        }
    }
}
