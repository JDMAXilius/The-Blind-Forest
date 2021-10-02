using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;
    public string exposedParam;
    public Slider slider;
    private void Start()
    {
        mixer.GetFloat(exposedParam, out float value);
        slider.value = Mathf.Pow(10,value/20);
    }
    public void SetMixerVolume(float sliderVal) {
        mixer.SetFloat(exposedParam, Mathf.Log10(sliderVal) * 20);
        mixer.GetFloat(exposedParam, out float val);
        PlayerPrefs.SetFloat(exposedParam, val);
    }
}
