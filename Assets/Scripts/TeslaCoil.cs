using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaCoil : MonoBehaviour
{
	[SerializeField]
	private Sprite idle;
	[SerializeField]
	private Sprite green;
	[SerializeField]
	private Sprite yellow;
	[SerializeField]
	private Sprite red;
	[SerializeField]
	private Sprite armed;

	[SerializeField]
	private AudioClip beepSfx;
	[SerializeField]
	private AudioClip activateSfx;

	private SpriteRenderer spriteRenderer;
	private float timeStarted;
	private enum State {
		IDLE,
		GREEN,
		YELLOW,
		RED,
		ARMED
	}
	private State state;

	void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		
		timeStarted = Time.time;
		state = State.IDLE;
	}

	void Update()
	{
		if(state == State.IDLE && Time.time - timeStarted > 8.0f)
		{
			SoundManager.instance.PlaySingleBacgroundSfx(beepSfx);
			spriteRenderer.sprite = green;
			state = State.GREEN;
		}
		else if(state == State.GREEN && Time.time - timeStarted > 9.0f)
		{
			SoundManager.instance.PlaySingleBacgroundSfx(beepSfx);
			spriteRenderer.sprite = yellow;
			state = State.YELLOW;
		}
		else if(state == State.YELLOW && Time.time - timeStarted > 10.0f)
		{
			SoundManager.instance.PlaySingleBacgroundSfx(beepSfx);
			spriteRenderer.sprite = red;
			state = State.RED;
		}
		else if(state == State.RED && Time.time - timeStarted > 11.0f)
		{
			SoundManager.instance.PlaySingleBacgroundSfx(beepSfx);
			spriteRenderer.sprite = armed;
			state = State.ARMED;
		}
	}

	public void Restart()
	{
		spriteRenderer.sprite = idle;
		state = State.IDLE;
		timeStarted = Time.time;
	}
}
