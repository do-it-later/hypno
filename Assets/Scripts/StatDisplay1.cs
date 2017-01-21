using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatDisplay1 : MonoBehaviour
{
	[SerializeField]
	private Image statImage;
	private RectTransform rect;

	void Start()
	{
		rect = statImage.rectTransform;
	}

	public void UpdateStat(float value)
	{
		Vector2 scale = transform.localScale;
		scale.x = Mathf.Lerp(1.0f, 1.5f, value /100);
		scale.y = Mathf.Lerp(1.0f, 1.5f, value /100);
		transform.localScale = scale;
	}
}