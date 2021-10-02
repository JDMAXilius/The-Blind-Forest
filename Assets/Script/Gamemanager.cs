using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Audio;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager Instance { get; private set; } // static singleton
    // player information
    public GameObject player;
    //public PlayerController playerScript;
    public PlayerController_UnityChan playerScript;

    // SpawnLocation information
    public Transform SpawnLocation;

    // HUD information
    public GameObject HUD;
    public HUDScript HUDScript;

    public GameObject animatorFunctions;
    public AnimatorFunctions animatorFunctionsScript;
    public string nextLevelName;

    public Vector3 lastCheckPointPos;
    public AudioMixer GlobalMixer;
    public GameObject textPopup;
    public TextPopup textPopupScript;

    [Header("References")]
    public Image inventoryItemImage;


    private void Awake()
    {
        Instance = this;
        GlobalMixer.SetFloat("MasterVol", PlayerPrefs.GetFloat("MasterVol"));
        GlobalMixer.SetFloat("MusicVol", PlayerPrefs.GetFloat("MusicVol"));
        GlobalMixer.SetFloat("AmbienceVol", PlayerPrefs.GetFloat("AmbienceVol"));
        GlobalMixer.SetFloat("SFXVol", PlayerPrefs.GetFloat("SFXVol"));
    }
    // Start is called before the first frame update
    void Start()
    {
        // player initialization
        //player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            //playerScript = player.GetComponent<PlayerController>();
        	playerScript = player.GetComponent<PlayerController_UnityChan>();
        }
        //lastCheckPointPos = GameObject.FindGameObjectWithTag("Respawn").transform.position;
        //player.transform.position = lastCheckPointPos;
        //Respawn();

        // HUD initialization
        HUD = GameObject.FindGameObjectWithTag("HUD");
        if (HUD)
        {
            HUDScript = HUD.GetComponent<HUDScript>();
        }

        //// SceneLoad initialization
        //SceneLoad = GameObject.FindGameObjectWithTag("SceneLoad");
        //SceneLoadScrip = SceneLoad.GetComponent<SceneLoadTrigger>();

        //animatorFunctions initialization
        //animatorFunctions = GameObject.FindGameObjectWithTag("HUD");
        if (animatorFunctions)
        {
            animatorFunctionsScript = animatorFunctions.GetComponent<AnimatorFunctions>();
        }
        textPopup = GameObject.FindGameObjectWithTag("TextPopup");
        if (textPopup) {
            textPopupScript = textPopup.GetComponent<TextPopup>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Respawn();
    }

    public void Respawn()
    {
        //player.transform.position = lastCheckPointPos;
        //Instantiate(player, SpawnLocation.position, Quaternion.identity);
    }
}
