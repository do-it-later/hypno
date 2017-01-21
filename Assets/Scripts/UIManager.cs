using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
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
    //float percent = 100;

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        // TODO: For testing. Delete or comment out! - JP
        //if (percent > 0)
        //{
        //    percent -= 0.1f;
        //    updateUIItem(2, UIObject.HEALTH, percent);
        //    updateUIItem(2, UIObject.CHARGE, percent);
        //    updateUIItem(1, UIObject.HEALTH, percent);
        //    updateUIItem(1, UIObject.CHARGE, percent);
        //}
	}

    // playerId: 1 to number of players    objectType: HEALTH/CHARGE    percentage: 0-100
    public void updateUIItem(int playerId, UIObject objectType, float percentage)
    {
        playerId = playerId - 1;
        if (playerId < 0 || playerId > 2 || percentage < 0 || percentage > 100)
        {
            return;
        }

        Image element = null;
        if (objectType == UIObject.HEALTH)
        {
            element = healthBars[playerId];
        }
        else if (objectType == UIObject.CHARGE)
        {
            element = chargeBars[playerId];
        }

        element.rectTransform.localScale = new Vector3(percentage / 100, element.rectTransform.localScale.y, element.rectTransform.localScale.z);
    }
}
