using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    //private GameObject player;
    // public Gamemanager gm;
    [SerializeField] bool isBossCheckPoint;
    [SerializeField] string bossCheckpointText;

    [SerializeField] private AudioClip collectionSound;
    public GameObject particule;

    // Start is called before the first frame update
    void Start()
    {
        //gm = GameObject.FindWithTag("GameController").GetComponent<Gamemanager>();

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject == Gamemanager.Instance.player)
        {
            if (collectionSound)
                Gamemanager.Instance.playerScript.sfxAudioSource.PlayOneShot(collectionSound, 1 * Random.Range(.8f, 1.4f));
            if (particule)
            {
                GameObject newparticule = (GameObject)Instantiate(particule, transform.position, Quaternion.identity);
                Destroy(newparticule, 3.5f);
            }
            ////debug.log("CheckPoint");
            Gamemanager.Instance.playerScript.spawnLocation = transform.position;
            //debug.log(Gamemanager.Instance.playerScript.spawnLocation);
            if (!isBossCheckPoint)
            {
                Gamemanager.Instance.textPopupScript.PrintText("Checkpoint Reached", 60);
            }
            else {
                Gamemanager.Instance.textPopupScript.PrintText(bossCheckpointText, 140);
            }
        }
    }
}
