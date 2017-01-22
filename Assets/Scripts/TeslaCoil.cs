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

	private SpriteRenderer spriteRenderer;

	void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();

		StartCoroutine(StartCoil());
	}

	IEnumerator StartCoil()
	{
		yield return new WaitForSeconds(8.0f);
		spriteRenderer.sprite = green;
		yield return new WaitForSeconds(1.0f);
		spriteRenderer.sprite = yellow;
		yield return new WaitForSeconds(1.0f);
		spriteRenderer.sprite = red;
		yield return new WaitForSeconds(1.0f);
		spriteRenderer.sprite = armed;
	}

	public void Restart()
	{
		spriteRenderer.sprite = idle;
		StartCoroutine(StartCoil());
	}
}
