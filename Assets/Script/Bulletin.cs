using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bulletin : MonoBehaviour
{
    public string textContent;
    Text text;
    public GameObject canvas;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        //text.text = textContent;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject == Gamemanager.Instance.player)
        {
            canvas.SetActive(true);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject == Gamemanager.Instance.player)
        {
            canvas.SetActive(false);
        }
    }
}
