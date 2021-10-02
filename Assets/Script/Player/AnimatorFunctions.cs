using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorFunctions : MonoBehaviour
{
    public static AnimatorFunctions Instance { get; private set; } // static singleton

    [Header("Particles")]
    [SerializeField] public ParticleSystem playStepEmitParticles;
    [SerializeField] private ParticleSystem playJumpEmitParticles;
    [SerializeField] private ParticleSystem playHitEmitParticles;
    [SerializeField] private ParticleSystem playLassertEmitParticles;
    [SerializeField] private ParticleSystem playOnHitEmitParticles;
    [SerializeField] private ParticleSystem playDashEmitParticles;
    [SerializeField] private ParticleSystem playDieEmitParticles;
    [SerializeField] private int emitAmount1=5;
    [SerializeField] private int emitAmount2=25;
   

    [Header("Sound Bank")]
    [SerializeField] private AudioClip[] sound1;
    [SerializeField] private float sound1Volume = 1;
    [SerializeField] private AudioClip[] playJumpSound;
    [SerializeField] private AudioClip[] playStepSound;
    [SerializeField] private AudioClip[] playHitSound;
    [SerializeField] private AudioClip[] playOnHitSound;
    [SerializeField] private AudioClip[] playDashSound;
    [SerializeField] private AudioClip[] playDieSound;
    [SerializeField] private float playJumpSoundf = .8f;
    [SerializeField] private float playStepSoundf = .3f;
    [SerializeField] private float playHitSoundf = 1;
    [SerializeField] private float playOnHitSoundf = 1;
    [SerializeField] private float playDashSoundf = 1;
    [SerializeField] private float playDieSoundf = .1f;

    [SerializeField] MenuButtonController menuButtonController;
    public bool disableOnce;

    public AudioSource sfxAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    void PlaySound(AudioClip whichSound)
    {
        if (!disableOnce)
        {
            menuButtonController.audioSource.PlayOneShot(whichSound);
        }
        else
        {
            disableOnce = false;
        }
    }
    public void PlayStepEmitParticles()
    {
        //playStepEmitParticles.Emit(emitAmount1);
        //Gamemanager.Instance.playerScript.ParticuleSource.playStepEmitParticles.Emit(emitAmount1);
        //Gamemanager.Instance.playerScript.ParticuleSource = playStepEmitParticles;
        Gamemanager.Instance.playerScript.ParticuleSource.Emit(emitAmount1);
    }
    public void PlayJumpEmitParticles()
    {
        playJumpEmitParticles.Emit(emitAmount1);
    }
    public void PlayHitEmitParticles()
    {
        playHitEmitParticles.Emit(emitAmount1);
    }
    public void PlayLasserEmitParticles()
    {
        playLassertEmitParticles.Emit(emitAmount1);
    }
    public void PlayOnHitEmitParticles()
    {
        playOnHitEmitParticles.Emit(emitAmount1);
    }
    public void PlayDashEmitParticles()
    {
        playDashEmitParticles.Emit(emitAmount1);
    }
    public void PlayDieEmitParticles()
    {
        playDieEmitParticles.Emit(emitAmount2);
        //GetComponent<EnemyAI>().ParticuleSource.Emit(emitAmount2);
    }

    public void PlayJumpSound()
    {
        Gamemanager.Instance.playerScript.sfxAudioSource.PlayOneShot(playJumpSound[Random.Range(0, sound1.Length)], sound1Volume);
    }

    public void PlayStepSound()
    {
        Gamemanager.Instance.playerScript.sfxAudioSource.PlayOneShot(playStepSound[Random.Range(0, sound1.Length)], playStepSoundf);
    }
    public void PlayHitSound()
    {
        Gamemanager.Instance.playerScript.sfxAudioSource.PlayOneShot(playHitSound[Random.Range(0, sound1.Length)], playHitSoundf);
    }
    public void PlayOnHitSound()
    {
        Gamemanager.Instance.playerScript.sfxAudioSource.PlayOneShot(playOnHitSound[Random.Range(0, sound1.Length)], playOnHitSoundf);
    }
    public void PlayDashSound()
    {
        Gamemanager.Instance.playerScript.sfxAudioSource.PlayOneShot(playDashSound[Random.Range(0, sound1.Length)], playDashSoundf);
    }
    public void PlayDieSound()
    {
        Gamemanager.Instance.playerScript.sfxAudioSource.PlayOneShot(playDieSound[Random.Range(0, sound1.Length)], playDieSoundf);
        sfxAudioSource.Play();
    }
    void PlaySound1()
    {
        Gamemanager.Instance.playerScript.sfxAudioSource.PlayOneShot(sound1[Random.Range(0, sound1.Length)], sound1Volume);
    }
}
