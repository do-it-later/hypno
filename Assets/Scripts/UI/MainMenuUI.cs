using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
	public GameObject mainMenuPage;
	public GameObject creditsPage;

	private ControllerInputManager controllerInputManager;

	void Start()
	{
		controllerInputManager = GetComponent<ControllerInputManager>();
	}

	void Update()
	{
		if(mainMenuPage.activeSelf && Input.GetKeyDown(controllerInputManager.GetButtonString(ControllerInputManager.Button.START)))
		{
			SceneManager.LoadScene("Game");
		}

		if(mainMenuPage.activeSelf && Input.GetKeyDown(controllerInputManager.GetButtonString(ControllerInputManager.Button.SELECT)))
		{
			mainMenuPage.SetActive(false);
			creditsPage.SetActive(true);
		}

		if(creditsPage.activeSelf && Input.GetKeyDown(controllerInputManager.GetButtonString(ControllerInputManager.Button.B)))
		{
			mainMenuPage.SetActive(true);
			creditsPage.SetActive(false);
		}
	}
}
