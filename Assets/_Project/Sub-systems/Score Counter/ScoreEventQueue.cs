using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ScorePopupFactory))]
public class ScoreEventQueue : MonoBehaviour
{
    private Queue<IScoreEvent> scoreEventQueue = new Queue<IScoreEvent>();

    private ScorePopupFactory scorePopupFactory;

    private void Start()
    {
        if (scorePopupFactory == null)
        {
            scorePopupFactory = GetComponent<ScorePopupFactory>();
        }
    }

    public void AddScoreEvent(IScoreEvent scoreEvent)
    {
        scoreEventQueue.Enqueue(scoreEvent);
    }

    private void FixedUpdate()
    {
        if (scoreEventQueue.Count > 0)
        {
            IScoreEvent scoreEvent = scoreEventQueue.Dequeue();
            ProcessEvent(scoreEvent);
        }
    }

    private void ProcessEvent(IScoreEvent scoreEvent)
    {
        switch (scoreEvent)
        {
            case ScoreIncreaseEvent scoreIncreaseEvent:
                scorePopupFactory.CreateScorePopup(scoreIncreaseEvent.score);
                break;
            default:
                throw new System.NotImplementedException();
        }
    }
}
