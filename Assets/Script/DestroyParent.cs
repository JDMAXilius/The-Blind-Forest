using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParent : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Gamemanager.Instance.playerScript.HP += 10;
            //debug.log("hp+10");
            Destroy(transform.parent.gameObject);
            //Destroy(gameObject);
        }
    }
}
