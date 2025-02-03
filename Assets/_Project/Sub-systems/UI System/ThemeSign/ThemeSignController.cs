using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ThemeSignController : BaseUIObject
{
    [Header("References/Misc")]
    [SerializeField] private TextMeshProUGUI themeSignText;
    [SerializeField] private Animator themeSignAnim;
    [SerializeField] private string themeSignTrigger = "themeDone";

    [Header("Animation Attributes")]
    [SerializeField] private int wordsToDisplay = 10;
    [SerializeField] private float timePerWord = 0.3f;
    [SerializeField] private string[] themeNames; // List of theme names, could be improved by taking names via the wordloader

    private int previousIndex = 0;
    private int currentIndex = 0;
    private int wordsDisplayed = 0;


    protected override void Awake()
    {
        base.Awake();
        themeSignText = GetComponentInChildren<TextMeshProUGUI>();
        themeSignAnim = GetComponent<Animator>();
    }

    public void Start()
    {
        StartCoroutine(DisplayThemeSign());
    }

    private IEnumerator DisplayThemeSign()
    {
        currentIndex = Random.Range(0, themeNames.Length);
        while (currentIndex == previousIndex && themeNames.Length > 1)
        {
            currentIndex = Random.Range(0, themeNames.Length);
        }
        previousIndex = currentIndex;
        themeSignText.text = themeNames[currentIndex];
        yield return new WaitForSeconds(timePerWord);

        timePerWord *= 1.1f;

        if (wordsDisplayed < wordsToDisplay)
        {
            wordsDisplayed++;
            StartCoroutine(DisplayThemeSign());
        }
        else
        {
            StartCoroutine(EndAnimationSequence());
        }
    }

    private IEnumerator EndAnimationSequence()
    {
        yield return new WaitForSeconds(timePerWord);
        themeSignText.text = ThemeHolder.Instance.ReturnTheme();
        yield return new WaitForSeconds(timePerWord);
        themeSignAnim.SetTrigger("themeDone");
    }
}