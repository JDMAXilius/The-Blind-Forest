using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelFadeIn : MonoBehaviour
{
    RawImage img;
    [SerializeField] float fadeTime;
    [SerializeField] float delayTime;
    bool paused;
    private void Awake()
    {
        img = GetComponent<RawImage>();
        img.color = new Vector4(255, 255, 255, 1);
    }
    // Start is called before the first frame update
    void Start()
    {
        //img = GetComponent<RawImage>();
        HUDScript.pauseGame += Pause;
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        while (delayTime > 0) {
            delayTime -= Time.deltaTime;
            yield return null;
        }
        for (float i = 0; i < fadeTime; i += Time.deltaTime)
        {
            if (!paused)
            {
                img.color = new Vector4(255, 255, 255, 1-(i / fadeTime));
            }
            else
            {
                i -= Time.deltaTime;
            }
            yield return null;
        }
        img.color = new Vector4(255, 255, 255, 0);
        Gamemanager.Instance.HUDScript.canPause = true;
        Destroy(gameObject);
    }
    public void Pause() {
        paused = !paused;
    }
}
