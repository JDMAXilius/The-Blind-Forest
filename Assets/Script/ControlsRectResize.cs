using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsRectResize : MonoBehaviour
{
    [SerializeField] RawImage parentImage;
    private void OnEnable()
    {
        parentImage.enabled = false;
    }
    private void OnDisable()
    {
        parentImage.enabled = true;
    }
}
