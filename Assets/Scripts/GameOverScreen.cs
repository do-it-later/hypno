using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour {
    public Text victoryText;
    public Text p1Accuracy;
    public Text p2Accuracy;
    public Text p1Shield;
    public Text p2Shield;
    public Text p1Reflect;
    public Text p2Reflect;
    public Text p1Name;
    public Text p2Name;

    public void updateVictoryText(string text, Color winner)
    {
        victoryText.text = text;
        victoryText.color = winner;
    }

    public void updatePlayerNameColors(Color p1, Color p2)
    {
        p1Name.color = p1;
        p2Name.color = p2;
    }

    public void updatePlayerAccuracy(float accuracy1, float accuracy2)
    {
        p1Accuracy.text = accuracy1.ToString("P");
        p2Accuracy.text = accuracy2.ToString("P");
    }

    public void updatePlayerReflected(int reflect1, int reflect2)
    {
        p1Reflect.text = reflect1.ToString();
        p2Reflect.text = reflect2.ToString();
    }

    public void updatePlayerShieldTime(float time1, float time2)
    {
        p1Shield.text = time1.ToString("0.00") + " s";
        p2Shield.text = time2.ToString("0.00") + " s";
    }

    public void UpdatePlayerNames(string p1, string p2)
    {
        p1Name.text = p1;
        p2Name.text = p2;
    }
}
