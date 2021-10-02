using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneKeys : MonoBehaviour
{
    enum ItemType { Health, Mana, InventoryItem } //Creates an ItemType enum (drop down)
    [SerializeField] private ItemType itemType;
    [SerializeField] private string inventoryStringName;
    [SerializeField] private Sprite inventorySprite;
    [SerializeField] private AudioClip collectionSound;
    [SerializeField] private float collectionSoundVolume = 1;
    public GameObject particule;

    [SerializeField] private ParticleSystem particlesCollectableGlitter;
    bool paused;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        HUDScript.pauseGame += Pause;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Gamemanager.Instance.player)
        {
            if (collectionSound) Gamemanager.Instance.playerScript.sfxAudioSource.PlayOneShot(collectionSound, collectionSoundVolume * Random.Range(.8f, 1.4f));

            if (particule)
            {
                GameObject newparticule = (GameObject)Instantiate(particule, transform.position + new Vector3(0, Random.Range(0, 3)), Quaternion.identity);
                Destroy(newparticule, 1);
            }

            if (itemType == ItemType.Mana)
            {
                Gamemanager.Instance.playerScript.Mana += 4;
            }
            else if (itemType == ItemType.Health)
            {
                if(Gamemanager.Instance.playerScript.HP < 100)
                {
                    Gamemanager.Instance.playerScript.HP += 4;
                }
            }
            else if (itemType == ItemType.InventoryItem)
            {
                Gamemanager.Instance.playerScript.AddInventoryItem(inventoryStringName, inventorySprite);
               
            }

            Destroy(gameObject);
        }
    }
    public void Pause() {
        paused = !paused;
        if (paused)
        {
            anim.enabled = false;
            if (particlesCollectableGlitter.isPlaying) {
                particlesCollectableGlitter.Pause();
            }
        }
        else {
            anim.enabled = true;
            if (particlesCollectableGlitter.isPaused)
            {
                particlesCollectableGlitter.Play();
            }
        }
    }
    private void OnDestroy()
    {
        HUDScript.pauseGame -= Pause;
    }
}
