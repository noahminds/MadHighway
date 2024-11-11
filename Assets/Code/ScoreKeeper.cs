using System;
using TMPro;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour {
    private static float score;
    private static TextMeshProUGUI scoreText;
    private static GameManager gameManager;

    internal void Start () {
        scoreText = GetComponent<TextMeshProUGUI>();
        gameManager = FindObjectOfType<GameManager>();
        UpdateText();
    }

    internal void Update () {
        if (score >= gameManager.maxScore)
        {
            gameManager.GameOver();
            score = gameManager.maxScore;
            UpdateText();
        }
    }

    public static void AddToScore(float points)
    {
        score += points;
        UpdateText();
    }

    public static void DecrementScore(float points)
    {
        score -= points;
        UpdateText();
    }

    public static void ResetScore()
    {
        score = 0;
        UpdateText();
    }

    private static void UpdateText()
    {
        scoreText.text = $"Score: {score}/{gameManager.maxScore}";
    }
}