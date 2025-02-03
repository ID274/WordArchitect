using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    private ScoreCounter scoreCounter;

    public int score { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        scoreCounter = FindObjectOfType<ScoreCounter>();
        ResetScore();
    }

    public void UpdateScore()
    {
        scoreCounter.UpdateScore(score);
    }

    public void ResetScore()
    {
        score = 0;
        UpdateScore();
    }

    public void IncrementScore()
    {
        score++;
        UpdateScore();
    }

    public void ActivateScoreCounter()
    {
        scoreCounter.ScoreCounterAppear();
    }

}
