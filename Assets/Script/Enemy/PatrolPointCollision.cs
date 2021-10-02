using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPointCollision : MonoBehaviour
{
    [SerializeField] GameObject enemySibling;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Enemy" && other.gameObject == enemySibling) {
            if (enemySibling.TryGetComponent<PatrolEnemy>(out PatrolEnemy p))
            {
                if (p.chasePlayer)
                {
                    p.chasePlayer = false;

                }
                p.SwapDirection();
            }
        }
    }
}
