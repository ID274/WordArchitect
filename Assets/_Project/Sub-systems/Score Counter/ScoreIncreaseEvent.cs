using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreIncreaseEvent : IScoreEvent
{
    public int score;
    public ScoreIncreaseEvent(int score)
    {
        this.score = score;
    }
}