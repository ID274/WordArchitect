using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
[RequireComponent(typeof(Animator))]
public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Animator scoreAnimator;
    private ScoreEventQueue scoreEventQueue;

    [SerializeField] private string animAppearTrigger = "counterAppear";
    [SerializeField] private string animUpdateTrigger = "counterUpdate";

    private void Awake()
    {
        scoreAnimator = GetComponent<Animator>();
        scoreText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        if (scoreEventQueue == null)
        {
            scoreEventQueue = FindObjectOfType<ScoreEventQueue>();
        }

        if (scoreEventQueue == null)
        {
            Debug.LogError($"Score event queue is null", this);
        }
    }

    private void OnEnable()
    {
        ScoreManager.OnScoreChanged += UpdateScore;
    }

    private void OnDisable()
    {
        ScoreManager.OnScoreChanged -= UpdateScore;
    }

    public void UpdateScore()
    {
        int score = ScoreManager.Instance.Score;
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
        else
        {
            Debug.Log($"Score text is null", this);
        }

        if (scoreAnimator != null)
        {
            scoreAnimator.SetTrigger(animUpdateTrigger);

            if (scoreEventQueue != null)
            {
                IScoreEvent scoreEvent = new ScoreIncreaseEvent(1); // 1 score
                scoreEventQueue.AddScoreEvent(scoreEvent);
            }
        }
        else
        {
            Debug.Log($"Score animator is null", this);
        }
    }

    public void ScoreCounterAppear()
    {
        scoreAnimator.SetTrigger(animAppearTrigger);
    }
}
