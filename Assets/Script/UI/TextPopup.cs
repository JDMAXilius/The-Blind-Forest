using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TextPopup : MonoBehaviour
{
    Text text;
    public string outputText;
    [SerializeField] float fadeLength;
    [SerializeField] float opaqueTextLength;
    bool paused;
    float prevOpacity;

    bool firstTime = true;
    float firstFadeLength = 1;
    float firstOpaqueTextLength = 2;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        text.color = new Vector4(255, 255, 255, 0);
        PrintText();
        HUDScript.pauseGame += Pause;
    }
    public void PrintText(string printText = "",int printFont = 70) {
        if (printText.Equals("")) {
            printText = outputText;
        }
        text.text = printText;
        text.fontSize = printFont;
        StartCoroutine(Fade());
        firstTime = false;
    }
    IEnumerator Fade() {
        float timer;
        float tempFade;
        if (firstTime)
        {
            timer = firstOpaqueTextLength;
            tempFade = firstFadeLength;
        }
        else {
            timer = opaqueTextLength;
            tempFade = fadeLength;
        }

        for (float i = 0; i < tempFade; i += Time.deltaTime) {
            if (!paused)
            {
                text.color = new Vector4(255, 255, 255, i / tempFade);
                prevOpacity = text.color.a;
            }
            else {
                i -= Time.deltaTime;
            }
            yield return null;
        }
        while (timer > 0) {
            if (!paused) {
                timer -= Time.deltaTime;
            }
            yield return null;
        }
        for (float i = 0; i < tempFade; i += Time.deltaTime)
        {
            if (!paused)
            {
                text.color = new Vector4(255, 255, 255, 1-(i/tempFade));
                prevOpacity = text.color.a;
            }
            else
            {
                i -= Time.deltaTime;
            }
            yield return null;
        }
        text.color = new Vector4(255, 255, 255, 0);
        prevOpacity = text.color.a;
    }
    public void Pause() {
        paused = !paused;
        if (paused)
        {
            text.color = new Vector4(255, 255, 255, 0);
        }
        else {
            prevOpacity = text.color.a;
        }
    }
    private void OnDestroy()
    {
        HUDScript.pauseGame -= Pause;
    }
}
