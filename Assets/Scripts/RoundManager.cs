using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RoundManager : MonoBehaviour {

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

	public GameObject gameOverDialog;
    private GameOverScreen gameOverScript;

	[SerializeField]
	private Text countdown3;
	[SerializeField]
	private Text countdown2;
	[SerializeField]
	private Text countdown1;
	[SerializeField]
	private Text countdownFight;

	[SerializeField]
	private AudioClip countdown3Sfx;
	[SerializeField]
	private AudioClip countdown2Sfx;
	[SerializeField]
	private AudioClip countdown1Sfx;
	[SerializeField]
	private AudioClip countdownFightSfx;

	void Start()
	{
		state = STATE.START;
		StartCoroutine(Countdown());
		gameOverScript = gameOverDialog.GetComponent<GameOverScreen>();
	}

	void Update() {
		var playersAlive = 0;
        var winner = players[0];
		players.ForEach(p => {
			if(p.activeInHierarchy)
            {
                playersAlive++;
                winner = p;
            } 
		});

		if(playersAlive < players.Count) {
			state = STATE.END;
		}

		if(state == STATE.END)
		{
            gameOverDialog.SetActive(true);
            var p1 = players[0].GetComponent<Player>();
            var p2 = players[1].GetComponent<Player>();
            gameOverScript.UpdatePlayerNames(p1.PlayerName, p2.PlayerName);
            gameOverScript.updatePlayerNameColors(players[0].GetComponent<SpriteRenderer>().color, players[1].GetComponent<SpriteRenderer>().color);
            if(playersAlive == 0)
            	gameOverScript.updateVictoryText("It's a tie!", Color.white);
            else
            	gameOverScript.updateVictoryText( winner.GetComponent<Player>().PlayerName + " wins!", winner.GetComponent<SpriteRenderer>().color);
            
            gameOverScript.updatePlayerAccuracy(p1.GetAccuracy(), p2.GetAccuracy());

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
		SoundManager.instance.PlaySingleBacgroundSfx(countdown3Sfx);
		countdown3.gameObject.SetActive(true);
		yield return new WaitForSeconds(1.0f);
		SoundManager.instance.PlaySingleBacgroundSfx(countdown2Sfx);
		countdown3.gameObject.SetActive(false);
		countdown2.gameObject.SetActive(true);
		yield return new WaitForSeconds(1.0f);
		SoundManager.instance.PlaySingleBacgroundSfx(countdown1Sfx);
		countdown2.gameObject.SetActive(false);
		countdown1.gameObject.SetActive(true);
		yield return new WaitForSeconds(1.0f);
		SoundManager.instance.PlaySingleBacgroundSfx(countdownFightSfx);
		countdown1.gameObject.SetActive(false);
		countdownFight.gameObject.SetActive(true);
		yield return new WaitForSeconds(1.0f);
		countdownFight.gameObject.SetActive(false);
		state = STATE.PLAYING;
	}
}
