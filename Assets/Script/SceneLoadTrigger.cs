using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadTrigger : MonoBehaviour
{
    //[SerializeField] public string loadSceneString;

    [SerializeField] private AudioClip collectionSound;
    public GameObject particule;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject == Gamemanager.Instance.player)
        {

            if (collectionSound)
                Gamemanager.Instance.playerScript.sfxAudioSource.PlayOneShot(collectionSound, 1 * Random.Range(.8f, 1.4f));
            if (particule)
            {
                GameObject newparticule = (GameObject)Instantiate(particule, transform.position + new Vector3(Random.Range(-2, 2), Random.Range(0, 3)), Quaternion.identity);
                Destroy(newparticule, 1);
            }
            Gamemanager.Instance.HUDScript.levelComplete = true;
            ////debug.log( Gamemanager.Instance.player.transform.position);
        }
    }
}