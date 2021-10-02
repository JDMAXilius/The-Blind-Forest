using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUDScript : MonoBehaviour
{
    //resource bars
    public Slider healthBar;
    public Slider manaBar;
    //public Button pauseButton;
    public GameObject pauseMenu = null;
    public string mainMenuScene;
    public GameObject gameOver;
    public GameObject completeEnding;
    public GameObject diedEnding;
    public GameObject options;
    public GameObject pauseButtons;
    public GameObject volume;
    public GameObject controls;
    public GameObject optionsButtons;
    public bool isOptions;
    public bool levelComplete = false;
    public bool isPauseButton { get; set; }
    public GameObject pauseButton;
    bool hasPausedAtEnding;
    public float youDiedDelay;
    public float youDiedOnScreen;
    public float fadeDelay;
    float runningTimer;
    bool died;
    public bool canPause = true;
    float gameOverAlpha;
    [SerializeField] EnemyAI bossEnemyScript;
    float bossMaxHP;
    [SerializeField] Slider bossHealth;
    [SerializeField] GameObject bossHealthObj;

    public delegate void PauseGame();
    public static PauseGame pauseGame;
    public string nextLevelName;

    //references
    //private GameObject player;
    //private GameObject GameManager;

    void Start()
    {
        //GameManager = GameObject.FindGameObjectWithTag("GameController");
        //player = GameManager.GetComponent<Gamemanager>().player;
        Time.timeScale = 1;
        gameOverAlpha = gameOver.GetComponent<RawImage>().color.a;
        if (bossEnemyScript)
        {
            bossMaxHP = bossEnemyScript.HP;
        }
    }
    void Update()
    {
        SetHealthBar();
        SetManaBar();
        CheckPause();
        CheckEnding();
        //if (runningTimer > 0)
        //{
        //    runningTimer -= Time.deltaTime;
        //}
        //else {
        //    if (!levelComplete)
        //    {
        //        gameOver.SetActive(false);
        //        diedEnding.SetActive(false);
        //        if (died == true)
        //            died = false;
        //    } 
        //}
    }

    public void SetHealthBar()
    {
        healthBar.value = Gamemanager.Instance.playerScript.HP;
        if (bossHealth)
        {
            if (bossEnemyScript && bossEnemyScript.HP > 0)
            {
                bossHealth.normalizedValue = bossEnemyScript.HP / bossMaxHP;
            }
            else {
                if (bossHealth)
                {
                    bossHealth.value = 0;
                    bossHealth.fillRect.sizeDelta = Vector2.zero;
                    bossHealth.fillRect.position.Set(-1000, -1000, 0);
                }
                if (!bossEnemyScript && bossHealthObj.activeSelf) {
                    bossHealthObj.SetActive(false);
                }
            }
        }
    }

    public void SetHealthBar(int Health)
    {
        healthBar.value = Health;
    }

    public void SetManaBar()
    {
        manaBar.value = Gamemanager.Instance.playerScript.Mana;
    }

    public void SetManaBar(int Mana)
    {
        manaBar.value = Mana;
    }

    //Function for when you click the pause button
    //public void OnPauseButtonClick()
    //{
    //    //debug.log("Pause");
    //}
    public void CheckPause() {
        if (!died && !levelComplete && canPause)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("start") || isPauseButton)
            {
                if (!isOptions)
                {
                    if (!pauseMenu.activeSelf)
                    {
                        if (pauseButton)
                        {
                            pauseButton.SetActive(false);
                            isPauseButton = false;
                        }
                        pauseMenu.SetActive(true);
                    }
                    else
                    {
                        if (pauseButton)
                        {
                            pauseButton.SetActive(true);
                            isPauseButton = false;
                        }
                        pauseMenu.SetActive(false);
                    }
                    pauseGame();
                }
                else {
                    OptionsMenu();
                }
            }
        }
    }
    public void OptionsMenu() {
        if (!options.activeSelf)
        {
            options.SetActive(true);
            isOptions = true;
            pauseButtons.SetActive(false);
        }
        else {
            if (!volume.activeSelf && !controls.activeSelf)
            {
                options.SetActive(false);
                isOptions = false;
                pauseButtons.SetActive(true);
            }
            else if (volume.activeSelf)
            {
                volume.SetActive(false);
                optionsButtons.SetActive(true);
            }
            else if (controls.activeSelf)
            {
                controls.SetActive(false);
                optionsButtons.SetActive(true);
            }
        }
    }
    public void CheckEnding() {
        if (Gamemanager.Instance.player)
        {
            //if (Gamemanager.Instance.playerScript.HP <= 0) {
                //gameOver.SetActive(true);
                //diedEnding.SetActive(true);
                //Time.timeScale = 0;
                //YouDied();
            //}
            //Complete Check
            if (levelComplete) {
                if (!hasPausedAtEnding)
                {
                    pauseGame();
                    hasPausedAtEnding = true;
                }
                gameOver.SetActive(true);
                completeEnding.SetActive(true);
                //Time.timeScale = 0;
            }
        }
    }

    public void YouDied()
    {
        if (!died)
        {
            died = true;
            StartCoroutine(DiedFade());
        }
    }
    IEnumerator DiedFade() {

        yield return new WaitForSeconds(youDiedDelay);
        RawImage goImg = gameOver.GetComponent<RawImage>();
        RawImage ydImg = diedEnding.GetComponent<RawImage>();
        goImg.color = new Vector4(255, 255, 255, 0);
        ydImg.color = new Vector4(255, 255, 255, 0);
        gameOver.SetActive(true);
        diedEnding.SetActive(true);
        pauseGame();
        for (float i = 0; i < fadeDelay; i += Time.deltaTime) {
            goImg.color = new Vector4(255, 255, 255, i/fadeDelay);
            ydImg.color = new Vector4(255, 255, 255, i/fadeDelay);
            yield return null;
        }
        goImg.color = new Vector4(255, 255, 255, 1);
        ydImg.color = new Vector4(255, 255, 255, 1);
        Gamemanager.Instance.player.transform.position = Gamemanager.Instance.playerScript.spawnLocation;
        yield return new WaitForSeconds(youDiedOnScreen);
        for (float i = 0; i < fadeDelay; i += Time.deltaTime)
        {
            goImg.color = new Vector4(255, 255, 255, 1-(i / fadeDelay));
            ydImg.color = new Vector4(255, 255, 255, 1-(i / fadeDelay));
            yield return null;
        }
        gameOver.SetActive(false);
        goImg.color = new Vector4(255, 255, 255, gameOverAlpha);
        diedEnding.SetActive(false);
        if (died == true)
        {
            died = false;
        }
        pauseGame();
    }

    public void OnResumeClick() {
        pauseMenu.SetActive(false);
    }
    public void OnNextLevelClick() {
        SceneManager.LoadScene(Gamemanager.Instance.nextLevelName);
        //SceneManager.LoadScene(nextLevelName);
    }
    public void OnRestartClick() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void OnMainMenuClick() {
        SceneManager.LoadScene(mainMenuScene);
    }
}
