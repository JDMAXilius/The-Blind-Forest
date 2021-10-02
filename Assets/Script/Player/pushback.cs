using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pushback : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            Vector2 direction = (other.transform.position - Gamemanager.Instance.player.transform.position).normalized;
            Gamemanager.Instance.playerScript.pushBack = -direction * 2;
            //debug.log("Enemy Entered Trigger");
        }

    }


}
