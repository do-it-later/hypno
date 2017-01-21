using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public Image[] healthBars;
    public Image[] chargeBars;
    public Image element;
    public enum UIObject
    {
        HEALTH,
        CHARGE
    };

    // TODO: For testing. Delete or comment out! - JP
    //float health = 100;

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        // TODO: For testing. Delete or comment out! - JP
        //if (health > 0)
        //{
        //    --health;
        //    updateUIItem(1, UIObject.HEALTH, health);
        //    updateUIItem(1, UIObject.CHARGE, health);
        //}
	}

    // playerId: 1 to number of players    objectType: HEALTH/CHARGE    percentage: 0-100
    public void updateUIItem(int playerId, UIObject objectType, float percentage)
    {
        if (playerId-1 < 0 || playerId-1 > 2 || percentage < 0 || percentage > 100)
        {
            return;
        }

        Image element = null;
        if (objectType == UIObject.HEALTH)
        {
            element = healthBars[playerId - 1];
        }
        else if (objectType == UIObject.CHARGE)
        {
            element = chargeBars[playerId - 1];
        }

        element.rectTransform.localScale = new Vector3(percentage / 100, element.rectTransform.localScale.y, element.rectTransform.localScale.z);
    }
}
