using NUnit.Framework;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.TestTools;

public class ScoreManagerTests
{
    ScoreManager scoreManager;

    [SetUp]
    public void Setup()
    {
        scoreManager = new GameObject().AddComponent<ScoreManager>();
    }

    [UnityTest]
    public IEnumerator IncrementScoreFiresEvent()
    {
        void AssertScoreChanged()
        {
            Assert.Pass("Score changed event fired");
        }
        ScoreManager.OnScoreChanged += AssertScoreChanged;

        scoreManager.IncrementScore();
        yield return null; // wait for next frame

    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(scoreManager.gameObject);
    }
}
