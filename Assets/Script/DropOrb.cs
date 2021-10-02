using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOrb : MonoBehaviour
{
    //public Transform target;
    public float minModifier = 7f;
    public float maxModifier = 11f;
    public bool isMana;
    public int[] HP = { 4, 13 };
    public int[] Mana = { 6, 13 };
    [SerializeField] private AudioClip collectionSound;
    public GameObject particule;

    Vector3 _velocity = Vector3.zero;
    bool _isFollowing = false;
    bool paused;
    // Start is called before the first frame update
    void Start()
    {
        HUDScript.pauseGame += Pause;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {
            if (_isFollowing)
            {
                transform.position = Vector3.SmoothDamp(transform.position, Gamemanager.Instance.player.transform.position + new Vector3(Random.Range(0, 2), Random.Range(0, 3)), ref _velocity, Time.deltaTime * Random.Range(minModifier, maxModifier));
            }
        }
    }

    public void StartFollow()
    {
        _isFollowing = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Gamemanager.Instance.player)
        {
            if (collectionSound)
            {
              Gamemanager.Instance.playerScript.sfxAudioSource.PlayOneShot(collectionSound, 1 * Random.Range(.8f, 1.4f));
            }
            if (particule)
            {
                GameObject newparticule = (GameObject)Instantiate(particule, transform.position + new Vector3(Random.Range(-2, 2), Random.Range(0, 3)), Quaternion.identity);
                Destroy(newparticule, 1);
            }
               
            if (isMana)
            {
                Gamemanager.Instance.playerScript.Mana += Random.Range(HP[0],HP[1]);
                ////debug.log("mana+4");
            }
            else
            {
                Gamemanager.Instance.playerScript.HP += Random.Range(Mana[0], Mana[1]);
                ////debug.log("hp+4");
            }
            //Destroy(transform.parent.gameObject);
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        HUDScript.pauseGame -= Pause;
    }
    void Pause() {
        paused = !paused;
    }
}
