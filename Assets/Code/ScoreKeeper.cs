using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour {
    private static float score;
    private static TextMeshProUGUI scoreText;

    internal void Start () {
        scoreText = GetComponent<TextMeshProUGUI>();
        UpdateText();
    }

    public static void AddToScore(float points)
    {
        score += points;
        UpdateText();
    }

    private static void UpdateText()
    {
        scoreText.text = String.Format("Score: {0}", score);
    }
}