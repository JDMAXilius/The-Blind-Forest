using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDashHitWall : MonoBehaviour
{
    [SerializeField] BossMain bossScript;
    [SerializeField] GameObject particleHit;
    [SerializeField] bool isLeft;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<BossMain>(out BossMain boss))
        {
            if (!bossScript.dazed)
            {
                Instantiate(particleHit, transform.position + (isLeft ? Vector3.right : Vector3.left), transform.rotation);
                bossScript.HitWall();
            }
        }
    }
}
