using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
	public GameObject mainMenuPage;
	public GameObject creditsPage;
    [SerializeField, Header("Controllers")]
    private ControllerInputManager cim1;
    [SerializeField]
    private ControllerInputManager cim2;

	void Update()
	{
		if(mainMenuPage.activeSelf)
		{
            if (Input.GetKeyDown(cim1.GetButtonString(ControllerInputManager.Button.START)) || Input.GetKeyDown(cim2.GetButtonString(ControllerInputManager.Button.START)))
			    SceneManager.LoadScene("Game");
            if (Input.GetKeyDown(cim1.GetButtonString(ControllerInputManager.Button.SELECT)) || Input.GetKeyDown(cim2.GetButtonString(ControllerInputManager.Button.SELECT)))
            {
                mainMenuPage.SetActive(false);
                creditsPage.SetActive(true);
            }
        }

		if(creditsPage.activeSelf && (Input.GetKeyDown(cim1.GetButtonString(ControllerInputManager.Button.B)) || Input.GetKeyDown(cim2.GetButtonString(ControllerInputManager.Button.B))))
		{
			mainMenuPage.SetActive(true);
			creditsPage.SetActive(false);
		}
	}
}
