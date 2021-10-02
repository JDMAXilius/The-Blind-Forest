using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
	[SerializeField] MenuButtonController menuButtonController;
	[SerializeField] Animator animator;
	[SerializeField] AnimatorFunctions animatorFunctions;
	[SerializeField] int thisIndex;
	private float submit;
   
	
	public int buildIndexToLoad;
	[SerializeField] GameObject buttonsToDisable;
	[SerializeField] GameObject loadingLabel;
	[SerializeField] LoadScene asyncLoad;
	[SerializeField] bool isResumeButton;
	[SerializeField] bool isQuit;
	[SerializeField] bool isOptions;
	[SerializeField] GameObject options;
	[SerializeField] GameObject disableTitle;
	[SerializeField] bool isSubOptions;
	[SerializeField] GameObject subOption;
	float delay;
	// Update is called once per frame
	void Update()
    {
		submit = Input.GetAxis("Submit");
		if (menuButtonController.index == thisIndex)
		{
			animator.SetBool("selected", true);
			if (delay <= 0)
			{
				if (submit == 1 || Input.GetAxis("Fire1") == 1 || Input.GetMouseButtonDown(0))
				{
					if (Input.GetMouseButtonDown(0))
					{
						if (menuButtonController.bounds1.Contains(menuButtonController.point) || menuButtonController.bounds2.Contains(menuButtonController.point) || menuButtonController.bounds3.Contains(menuButtonController.point) || menuButtonController.bounds4.Contains(menuButtonController.point))
						{
							animator.SetBool("pressed", true);
						}
					}
					else
					{
						if (!Input.GetMouseButton(0))
						{
							animator.SetBool("pressed", true);
						}
					}
				}
				else if (animator.GetBool("pressed"))
				{
					animator.SetBool("pressed", false);
					animatorFunctions.disableOnce = true;
				}
			}
		}
		else
		{
			animator.SetBool("selected", false);
		}
		if (delay > 0) {
			delay -= Time.deltaTime;
		}
		//if (menuButtonController.index == 0)
		//{
		//	if (Input.GetAxis("Submit") == 1 || Input.GetMouseButtonDown(0))
		//	{
		//		animator.SetBool("pressed", true);
		//		//PlayGame();

		//	}
		//}
		//if (menuButtonController.index == 2)
		//{
		//	if (Input.GetAxis("Submit") == 1 || Input.GetMouseButtonDown(0))
		//		if (Input.GetMouseButtonDown(0)) {
		//                  if(menuButtonController.bounds3.Contains(menuButtonController.point)){
		//				if (isQuit)
		//				{
		//					QuitGame();
		//				}
		//				else {
		//					PlayGame();
		//				}
		//			}
		//		}
		//		else if(Input.GetAxis("Submit") == 1)
		//              {
		//			if (isQuit)
		//			{
		//				QuitGame();
		//			}
		//			else {
		//				PlayGame();
		//			}
		//              }

		//}


		if (submit >= 1)
		{
			submit = 0;
		}
	}
	public void PlayGame()
	{
		if (isQuit)
		{
			QuitGame();
		}
		else if (isOptions)
		{
			if (Gamemanager.Instance.HUD)
			{
				Gamemanager.Instance.HUDScript.OptionsMenu();
			}
			else
			{
				if (options.activeSelf)
				{
					if (options)
					{
						options.SetActive(false);
					}
					if (buttonsToDisable)
					{
						buttonsToDisable.SetActive(true);
					}
					if (disableTitle)
					{
						disableTitle.SetActive(true);
					}
				}
				else
				{
					if (options)
					{
						options.SetActive(true);
					}
					if (buttonsToDisable)
					{
						buttonsToDisable.SetActive(false);
					}
					if (disableTitle)
					{
						disableTitle.SetActive(false);
					}
				}
			}
		}
		else if (isSubOptions) {
			if (subOption) {
				if (subOption.activeSelf) {
					subOption.SetActive(false);
				}
                else{
					subOption.SetActive(true);
				}
			}
			if (buttonsToDisable)
			{
				if (buttonsToDisable.activeSelf)
				{
					buttonsToDisable.SetActive(false);
				}
				else {
					buttonsToDisable.SetActive(true);
				}
			}
		}
		else
		{
			if (buttonsToDisable)
			{
				buttonsToDisable.SetActive(false);
			}
			if (loadingLabel)
			{
				loadingLabel.SetActive(true);
			}
			if (asyncLoad)
			{
				asyncLoad.buildIndex = buildIndexToLoad;
				asyncLoad.PlayGame();
			}
			if (isResumeButton)
			{
				HUDScript.pauseGame();
			}
		}
	}
    private void OnEnable()
    {
		delay = .3f;
    }
    public void QuitGame()
	{
		// SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
		//animator.SetBool("pressed", true);
		Application.Quit();
	}

	public void SetIndex()
	{
		menuButtonController.index = thisIndex;

	}
	public void Click()
	{
		submit += 1;
	}

}
