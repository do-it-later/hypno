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

	void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();

		StartCoroutine(StartCoil());
	}

	IEnumerator StartCoil()
	{
		yield return new WaitForSeconds(8.0f);
		SoundManager.instance.PlaySingleSfx(beepSfx);
		spriteRenderer.sprite = green;
		yield return new WaitForSeconds(1.0f);
		SoundManager.instance.PlaySingleSfx(beepSfx);
		spriteRenderer.sprite = yellow;
		yield return new WaitForSeconds(1.0f);
		SoundManager.instance.PlaySingleSfx(beepSfx);
		spriteRenderer.sprite = red;
		yield return new WaitForSeconds(1.0f);
		SoundManager.instance.PlaySingleSfx(activateSfx);
		spriteRenderer.sprite = armed;
	}

	public void Restart()
	{
		spriteRenderer.sprite = idle;
		StartCoroutine(StartCoil());
	}
}
