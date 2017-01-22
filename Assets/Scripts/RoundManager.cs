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
	}

	void Update() {
		var playersAlive = 0;
		players.ForEach(p => {
			if(p.activeInHierarchy) 
				playersAlive++;
		});

		if(playersAlive < players.Count) {
			state = STATE.END;
		}

		if(state == STATE.END)
		{
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
