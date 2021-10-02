using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCamera : MonoBehaviour
{
    //[SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    public Vector3 cameraWorldSize;
    public CinemachineFramingTransposer cinemachineFramingTransposer;
    private CinemachineBasicMultiChannelPerlin multiChannelPerlin;
    public float screenYDefault;
    public float screenYTalking;
    [Range(0, 10)]
    [System.NonSerialized] public float shakeLength = 10;
    private CinemachineVirtualCamera virtualCamera;

    // Start is called before the first frame update
    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();

        //Ensures we can shake the camera using Cinemachine. Don't really worry too much about this weird stuff. It's just Cinemachine's variables.
        cinemachineFramingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        screenYDefault = cinemachineFramingTransposer.m_ScreenX;

        //Inform the player what CameraEffect it should be controlling, no matter what scene we are on.
        Gamemanager.Instance.playerScript.sceneCamera = this;
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        multiChannelPerlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        multiChannelPerlin.m_FrequencyGain = 0;

        //Tells the virtualCamera what to follow
        virtualCamera.Follow = Gamemanager.Instance.player.transform;
    }

    void Update()
    {
        multiChannelPerlin.m_FrequencyGain += (0 - multiChannelPerlin.m_FrequencyGain) * Time.deltaTime * (10 - shakeLength);
        
    }

    public void Shake(float shake, float length)
    {
        shakeLength = length;
        multiChannelPerlin.m_FrequencyGain = shake;
    }
}
