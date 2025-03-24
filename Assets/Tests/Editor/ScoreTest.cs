using NUnit.Framework;
using UnityEngine;

public class ScoreTest
{
    GameObject testObject;
    ScoreManager scoreManager;
    ScoreCounter scoreCounter;

    [SetUp]
    public void SetUp()
    {
        testObject = new GameObject();
        scoreManager = testObject.AddComponent<ScoreManager>();
        scoreCounter = testObject.AddComponent<ScoreCounter>();
        scoreManager.AssignScoreCounter(scoreCounter);
    }

    [Test]
    public void ScoreResetTest()
    {
        scoreManager.Score += 10;
        Debug.Log($"Score: {scoreManager.Score}");
        Assert.AreEqual(scoreManager.Score, 10);
    }

    [Test]
    public void ScoreIncrementTest()
    {
        scoreManager.IncrementScore();
        Debug.Log($"Score: {scoreManager.Score}");
        Assert.AreEqual(scoreManager.Score, 1);
    }

    [TearDown]
    public void TearDown()
    {
        testObject = null;
        scoreManager = null;
        scoreCounter = null;
    }
}
