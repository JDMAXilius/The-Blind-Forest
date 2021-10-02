using System.Collections;
using System.Collections.Generic;
using UnityEngine;
enum EventType { 
    Translate,
    Rotate,
    Scale,
    TurnOffOneTimeUse,

}
public class SwitchEvent : MonoBehaviour
{
    public Switch switchScript;
    [SerializeField] float eventTime;
    float runningEventTime;
    [SerializeField] EventType eventType;
    bool canExecuteEvent = true;
    bool paused;

    [Space]
    [Header("Translate")]
    [SerializeField] Vector3 translation;

    [Space]
    [Header("Rotation")]
    [SerializeField] Vector3 rotation;

    [Space]
    [Header("Scaling")]
    [SerializeField] Vector3 scaling;
    // Start is called before the first frame update
    void Start()
    {
        HUDScript.pauseGame += Pause;
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {
            if (switchScript)
            {
                if (canExecuteEvent)
                {
                    if (switchScript.flipWasNotEqual)
                    {
                        if (switchScript.flipped)
                        {
                            StartCoroutine(TriggeredEvent());
                        }
                        else
                        {
                            StartCoroutine(TriggeredEvent(-1));
                        }
                    }
                }
            }
        }
    }

     public IEnumerator TriggeredEvent(int direction = 1) {
        canExecuteEvent = false;
        runningEventTime = eventTime;
        while (runningEventTime > 0)
        {
            if (!paused)
            {
                if (eventType == EventType.Translate)
                {
                    transform.Translate(translation * Time.deltaTime * direction);
                }
                else if (eventType == EventType.Rotate)
                {
                    transform.Rotate(rotation * Time.deltaTime * direction);
                }
                else if (eventType == EventType.Scale)
                {
                    transform.localScale += (scaling * Time.deltaTime * direction);
                }
                else
                {
                    gameObject.SetActive(false);
                }
                runningEventTime -= Time.deltaTime;
            }
            yield return null;
        }
        canExecuteEvent = true;
        if (switchScript)
        {
            switchScript.canFlip = true;
        }
    }
    private void OnDestroy()
    {
        HUDScript.pauseGame -= Pause;
    }
    void Pause() {
        paused = !paused;

    }
}
