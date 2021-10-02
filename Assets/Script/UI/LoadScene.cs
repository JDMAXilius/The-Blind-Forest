using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    public int buildIndex;
    float passedTime;
    [SerializeField] GameObject loading;
    Text loadingText;
    [SerializeField] RawImage blackScreen;
    AsyncOperation asyncLoad;
    // Start is called before the first frame update
    void Start()
    {
        loadingText = loading.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame()
    {
        StartCoroutine(LoadGame());
    }

    public void ReturnGame()
    {
       // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    IEnumerator LoadGame() {
        asyncLoad = SceneManager.LoadSceneAsync(buildIndex);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone) {
            if (asyncLoad.progress < .9f)
            {
                passedTime += Time.deltaTime;
                if (passedTime >= 3 && passedTime < 10)
                {
                    loadingText.text = "Still Loading...";
                }
                else if (passedTime >= 10)
                {
                    loadingText.text = "WOW! This is a long time...  " + asyncLoad.progress * 100 + "%";
                }
            }
            else {
                StartCoroutine(FadeOut());
            }
            yield return null;
        }
        //SceneManager.LoadScene("PresentationLevel");
    }

    IEnumerator FadeOut() {
        float increaseOpacityBy = .0001f;
        while(blackScreen.color.a < 1) {
            Color c = blackScreen.color;
            c.a += increaseOpacityBy;
            blackScreen.color = c;
            yield return null;
        }
        asyncLoad.allowSceneActivation = true;
    }
}
