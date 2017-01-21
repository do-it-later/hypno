using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class RoundManager : MonoBehaviour {

	private enum STATE {
		START,
		PLAYING,
		END
	}

	[SerializeField]
	private List<GameObject> players;
	private STATE state;
	[SerializeField]
	private ControllerInputManager cim1;
	[SerializeField]
	private ControllerInputManager cim2;
	[SerializeField]
	private UnityEvent OnRestart = new UnityEvent();

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
		state = STATE.PLAYING;
	}
}
