using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderSelect : MonoBehaviour
{
    Selectable slider;
    [SerializeField] MenuButtonController controller;
    [SerializeField] Image handle;
    [SerializeField] AudioClip clip;
    [SerializeField] int index;
    bool selectedOnce;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Selectable>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (index == controller.index)
        {
            slider.Select();
            handle.color = new Vector4(255, 0, 0, 1);
            if (!selectedOnce && clip)
            {
                controller.audioSource.PlayOneShot(clip);
                selectedOnce = true;
            }
        }
        else {
            handle.color = new Vector4(255, 255, 255, 1);
            selectedOnce = false;
        }
    }
    public void SetIndex() {
        controller.index = index;
    }
}
