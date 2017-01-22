using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour {
    public Text victoryText;
    public Text p1Accuracy;
    public Text p2Accuracy;

    public void updateVictoryText(string text)
    {
        victoryText.text = text;
    }

    public void updatePlayerAccuracy(float accuracy1, float accuracy2)
    {
        p1Accuracy.text = "Accuracy: " + accuracy1;
        p2Accuracy.text = "Accuracy: " + accuracy2;
    }
}
