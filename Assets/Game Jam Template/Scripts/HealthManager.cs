using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public Image healthBar;

    // TODO: For testing. Delete or comment out! - JP
    float health = 100;

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        --health;
        updateHealth(0, health);
	}

    public void updateHealth(int playerId, float health)
    {
        if (health >= 0 && health <= 100)
        {
            healthBar.rectTransform.localScale = new Vector3(health/100, healthBar.rectTransform.localScale.y, healthBar.rectTransform.localScale.z);
        }
    }
}
