using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawn : MonoBehaviour
{
    [SerializeField] GameObject box;
    [SerializeField] float maxDelaySpawnTime;
    [SerializeField] BossMain boss;
    float runningDelay;
    [SerializeField] bool healthBased;

    private void Start()
    {
        runningDelay = Random.Range(3, maxDelaySpawnTime);
    }
    // Update is called once per frame
    void Update()
    {
        if (!healthBased)
        {
            if (runningDelay > 0)
            {
                runningDelay -= Time.deltaTime;
            }
            else
            {
                runningDelay = Random.Range(3, maxDelaySpawnTime);
                if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 15))
                {
                    if (!hit.collider.gameObject.name.Contains("DestructableBox"))
                    {
                        Instantiate(box, transform);
                    }
                }
                else {
                    Instantiate(box, transform);
                }
                
            }
        }
        else {
            if (Gamemanager.Instance.playerScript.HP <= 25 || Gamemanager.Instance.playerScript.Mana <= 25) {
                if (runningDelay > 0)
                {
                    runningDelay -= Time.deltaTime;
                }
                else
                {
                    runningDelay = Random.Range(5, maxDelaySpawnTime);
                    if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit)) {
                        if (!hit.collider.gameObject.name.Contains("DestructableBox")) { 
                            GameObject boxSpawn = Instantiate(box, transform);
                            boxSpawn.layer = 0;
                        }
                    }
                }
            }
        }
        if (boss) {
            if (healthBased && boss.finalStage) {
                gameObject.SetActive(false);
            }
        }
    }
}
