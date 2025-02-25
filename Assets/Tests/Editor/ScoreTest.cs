using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ScoreTest
{
    GameObject testObject = new GameObject();
    ScoreManager scoreManager;
    ScoreCounter scoreCounter;

    [SetUp]
    public void SetUp()
    {
        scoreManager = testObject.AddComponent<ScoreManager>();
        scoreCounter = testObject.AddComponent<ScoreCounter>();
        scoreManager.AssignScoreCounter(scoreCounter);
    }

    [Test]
    public void ScoreResetTest()
    {
        scoreManager.Score = 100;
        scoreManager.ResetScore();
        Debug.Log($"Score: {scoreManager.Score}");
        Assert.AreEqual(scoreManager.Score, 0);
    }

    [Test]
    public void ScoreIncrementTest()
    {
        scoreManager.ResetScore();
        scoreManager.IncrementScore();
        Debug.Log($"Score: {scoreManager.Score}");
        Assert.AreEqual(scoreManager.Score, 1);
    }
}
