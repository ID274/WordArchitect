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

    [SerializeField] private string animAppearTrigger = "counterAppear";
    [SerializeField] private string animUpdateTrigger = "counterUpdate";

    private void Awake()
    {
        scoreAnimator = GetComponent<Animator>();
        scoreText = GetComponent<TextMeshProUGUI>();
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
