using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    private ScoreCounter scoreCounter;

    private int score = 0;

    // Here we use a property to return/change the score, but to also make sure every time it is changed, to call the UpdateScore() method.
    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
            UpdateScore();
        }
    }

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

    public void AssignScoreCounter(ScoreCounter scoreCounter) // method to enable unit testing of this class
    {
        this.scoreCounter = scoreCounter;
    }

    private void Start()
    {
        if (scoreCounter == null)
        {
            AssignScoreCounter(FindObjectOfType<ScoreCounter>());
        }
        ResetScore();
    }

    public void UpdateScore()
    {
        scoreCounter.UpdateScore(Score);
    }

    public void ResetScore()
    {
        Score = 0;
    }

    public void IncrementScore()
    {
        Score++;
    }

    public void ActivateScoreCounter()
    {
        scoreCounter.ScoreCounterAppear();
    }
}