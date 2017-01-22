using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RoundManager : MonoBehaviour {

    public GameObject gameOverDialog;
    public GameOverScreen gameOverScript;

	public enum STATE {
		START,
		PLAYING,
		END
	}

	[SerializeField]
	private List<GameObject> players;
	private STATE state;
	public STATE State { get { return state; } }
	[SerializeField]
	private ControllerInputManager cim1;
	[SerializeField]
	private ControllerInputManager cim2;
	[SerializeField]
	private UnityEvent OnRestart = new UnityEvent();

	[SerializeField]
	private Text countdown3;
	[SerializeField]
	private Text countdown2;
	[SerializeField]
	private Text countdown1;
	[SerializeField]
	private Text countdownFight;

	void Start()
	{
		state = STATE.START;
		StartCoroutine(Countdown());
	}

	void Update() {
		var playersAlive = 0;
        var winner = "";
		players.ForEach(p => {
			if(p.activeInHierarchy)
            {
                playersAlive++;
                winner = p.name;
            } 
		});

		if(playersAlive < players.Count) {
			state = STATE.END;
		}

		if(state == STATE.END)
		{
            gameOverDialog.SetActive(true);
            gameOverScript.updateVictoryText(winner+" wins!");
            var p1 = players[0].GetComponent<Player>();
            var p2 = players[1].GetComponent<Player>();
            if (p1.shotsHit > 0)
            {
                gameOverScript.updatePlayerAccuracy(1, p1.shotsFired / p1.shotsHit);
            }

            if (p2.shotsHit > 0)
            {
                gameOverScript.updatePlayerAccuracy(2, p2.shotsFired / p2.shotsHit);
            }

			if(Input.GetKeyDown(cim1.GetButtonString(ControllerInputManager.Button.START)))
			{
				RestartGame();
			}

			if(Input.GetKeyDown(cim2.GetButtonString(ControllerInputManager.Button.START)))
			{
				RestartGame();
			}
		}
	}

	private void RestartGame()
	{
		OnRestart.Invoke();
		state = STATE.START;

		StartCoroutine(Countdown());
        gameOverDialog.SetActive(false);
	}

	IEnumerator Countdown()
	{
		countdown3.gameObject.SetActive(true);
		yield return new WaitForSeconds(1.0f);
		countdown3.gameObject.SetActive(false);
		countdown2.gameObject.SetActive(true);
		yield return new WaitForSeconds(1.0f);
		countdown2.gameObject.SetActive(false);
		countdown1.gameObject.SetActive(true);
		yield return new WaitForSeconds(1.0f);
		countdown1.gameObject.SetActive(false);
		countdownFight.gameObject.SetActive(true);
		yield return new WaitForSeconds(1.0f);
		countdownFight.gameObject.SetActive(false);
		state = STATE.PLAYING;
	}
}
