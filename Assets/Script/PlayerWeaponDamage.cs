using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponDamage : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.GetComponent<EnemyAI>().takeDamage(Gamemanager.Instance.playerScript.attackPower);
            //debug.log("enemy hitting");
            //Physics.IgnoreLayerCollision(6, 7, true);
        }
    }
}