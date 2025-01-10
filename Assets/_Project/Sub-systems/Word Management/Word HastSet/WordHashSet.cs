using System.Collections.Generic;
using UnityEngine;

public class WordHashSet : MonoBehaviour
{
    public static WordHashSet Instance { get; private set; }

    [SerializeField] private bool dontDestroyOnLoad = true;

    private HashSet<string> wordHashSet = new HashSet<string>();

    [SerializeField] private string[] wordsPreview;

    private void Awake()
    {
        if (Instance != this && Instance != null)
        {
            Debug.LogWarning($"SELF ERROR: wordHashSet already exists, deleting {this.gameObject.name}.", this.gameObject);
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        if (dontDestroyOnLoad)
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void AddWord(string word)
    {
        word = word.ToUpperInvariant();
        wordHashSet.Add(word);
        wordsPreview = new string[wordHashSet.Count];
        wordHashSet.CopyTo(wordsPreview);
    }

    public void RemoveWord(string word)
    {
        word = word.ToUpperInvariant();
        wordHashSet.Remove(word);
    }

    public bool ContainsWord(string word)
    {
        word = word.ToUpperInvariant();
        return wordHashSet.Contains(word);
    }
}