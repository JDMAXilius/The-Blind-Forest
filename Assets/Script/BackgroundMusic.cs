using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private Animator animator;
    private AudioSource sfxAudioSource;
    [SerializeField] private AudioClip collectionSound;
    [SerializeField] private float collectionSoundVolume = 1;
    public float waitTime;
    enum ScenarioType { MainMenu, LevelStart, LevelMidle, LevelEnd } //Creates an ItemType enum (drop down)
    [SerializeField] private ScenarioType scenariType;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        sfxAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (scenariType == ScenarioType.MainMenu)
        {
            StartCoroutine(ChangeMusic());
            sfxAudioSource.PlayOneShot(collectionSound, collectionSoundVolume * Random.Range(.8f, 1.4f));
        }
    }

    IEnumerator ChangeMusic()
    {
        animator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(waitTime);
    }

    //internal void UpdateScenarioState(ScenarioType newState)
    //{
    //    if (newState != scenariType)
    //    {
    //        animator.Play(newState);
    //        currentState = newState;
    //    }

    //}
}
