using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveCollision : MonoBehaviour
{
    FlyingEnemy flyingParent;
    PatrolEnemy patrolParent;
    SlamEnemy slamParent;
    BossMain bossParent;
    public bool canHurt = true;
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            flyingParent = GetComponentInParent<FlyingEnemy>();
        }
        catch { }
        try
        {
            patrolParent = GetComponentInParent<PatrolEnemy>();
        }
        catch { }
        try
        {
            slamParent = GetComponentInParent<SlamEnemy>();
        }
        catch { }
        try
        {
            bossParent = GetComponentInParent<BossMain>();
        }
        catch { }
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            canHurt = false;
            if (flyingParent)
            {
                flyingParent.Contact(other);
            }
            else if (patrolParent)
            {
                patrolParent.chasePlayer = true;
            }
            else if (slamParent)
            {
                slamParent.Contact(other);
            }
            else if (bossParent) {
                bossParent.Contact(other);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")) {
            if (bossParent && canHurt) {
                bossParent.Contact(other);
                canHurt = false;
            }
        }
    }
}
