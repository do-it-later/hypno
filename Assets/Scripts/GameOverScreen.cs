using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour {
    public Text victoryText;
    public Text p1Accuracy;
    public Text p2Accuracy;

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
	}

    public void updateVictoryText(string text)
    {
        victoryText.text = text;
    }

    public void updatePlayerAccuracy(int playerId, float accuracy)
    {
        if (playerId == 1)
        {
            p1Accuracy.text = "Accuracy: " + accuracy;
        }
        else
        {
            p2Accuracy.text = "Accuracy: " + accuracy;
        }
    }
}
