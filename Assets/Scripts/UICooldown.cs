using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICooldown : MonoBehaviour
{
	[SerializeField]
	private Image dashImage;
    
    // ENSURE THAT VALUE IS BETWEEN 0 AND 1!
	public void UpdateDashCooldown(float value)
	{
        dashImage.fillAmount = 1 - value;
	}
}