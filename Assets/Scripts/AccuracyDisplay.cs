using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccuracyDisplay : MonoBehaviour
{
	[SerializeField]
	private Text text;

	public void UpdateAccuracy(float value)
	{
		Debug.Log(value);
		text.text = "Accuracy: " + value.ToString("P");
	}
}