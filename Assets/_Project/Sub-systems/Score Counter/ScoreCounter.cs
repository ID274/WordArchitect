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
        scoreText = GetComponent<TextMeshProUGUI>();
        scoreAnimator = GetComponent<Animator>();
    }

    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
        scoreAnimator.SetTrigger(animUpdateTrigger);
    }

    public void ScoreCounterAppear()
    {
        scoreAnimator.SetTrigger(animAppearTrigger);
    }
}
