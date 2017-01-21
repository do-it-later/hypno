using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatDisplay : MonoBehaviour
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
		rect.localScale = new Vector3(value / 100, rect.localScale.y, rect.localScale.z);
	}
}