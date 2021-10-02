using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private bool autoPlay; //Begins playing sound immediately without the player triggering the collider
    [SerializeField] private bool controlsTitle; //This allows the level title to fade in while also fading in the music
    [SerializeField] private float fadeSpeed; //How fast do we increase the volume? If set to 0, it will just play at a volume of 1
    [SerializeField] private bool loop;
    [SerializeField] private bool playOnce; //If set to true, the sound will only play once if triggered
    [SerializeField] private AudioClip sound;
    public float maxVolume; //The volume we are going to fade to
    private bool triggered; //Is set to true once the player touches the collider trigger zone
    private bool played; //Is set to true once the sound plays

    // Use this for initialization
    void Start()
    {
        Reset(false, sound, 0);
       // StartCoroutine(EnableCollider());
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.loop = loop;

        /*If the player isn't dead, and we either trigger or want to 
        AudioTrigger to automatically play, the audioSource will begin playing.
        */

        
            if (Gamemanager.Instance.playerScript.HP > 0)
            {
                if (triggered /*|| autoPlay*/)
                {
                    if (!audioSource.isPlaying && !played)
                    {
                        audioSource.PlayOneShot(sound);
                        played = true;
                    }

                    //Begin fading in the audioSource volume as long as it's smaller than the goToVolume
                    if (audioSource.volume < maxVolume && fadeSpeed != 0)
                    {
                        audioSource.volume += fadeSpeed * Time.deltaTime;
                    }
                }
                else
                {
                    if (audioSource.volume > 0 && fadeSpeed != 0)
                    {
                        audioSource.volume -= fadeSpeed * Time.deltaTime;
                    }
                    else if (fadeSpeed != 0)
                    {
                        audioSource.Stop();
                    }
                }
            }
            else
            {
                //audioSource.Stop();
            }
       
       
    }

    private void OnTriggerStay(Collider other)
    {  
          
       if (other.CompareTag("Player"))
       {
            if (!triggered)
            {
                if (controlsTitle)
                {
                    //Control titles here (will write in future video!)
                }

                triggered = true;
            }
       }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            triggered = false;
            if (!playOnce) played = false;
        }
    }

    //Find the audioSource, set it's volume and clip, and determine if it should start or stop playing.
    public void Reset(bool play, AudioClip clip, float startVolume = 1)
    {
        audioSource = GetComponent<AudioSource>();
        if (fadeSpeed != 0) audioSource.volume = startVolume;
        audioSource.clip = clip;
        if (play)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(sound);
        }
    }

    private IEnumerator EnableCollider()
    {
        /*If the player spawns inside a large trigger area, it won't trigger. Therefore, we wait 4 seconds 
        to actually enable it so the trigger can actually occur 
        */
        yield return new WaitForSeconds(4f);
        GetComponent<BoxCollider>().enabled = true;
    }
}