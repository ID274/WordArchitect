using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ColourChanger))]
public class WordSearchManager : BaseObserverSubject<IBlockObserver>, IBlockObserverSubject
{
    public static WordSearchManager Instance { get; private set; }

    private Trie wordTrie;
    private ColourChanger colourChanger;
    private List<string> stringsToType = new List<string>();

    private WordLoader wordLoader;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        wordTrie = new Trie();
        colourChanger = GetComponent<ColourChanger>();
    }

    private void Start()
    {
        // Load random word list
        if (TryGetComponent(out WordLoader wLoader))
        {
            wordLoader = wLoader;
            wordLoader.LoadRandomWordList();
        }
        else
        {
            Debug.LogError("No WordLoader found.", this);
        }
    }

    public void PopulateTrie(string[] wordsArray)
    {
        // Populate the wordTrie

        Debug.Log(wordsArray.Length + " words to add to trie");

        foreach (string word in wordsArray)
        {
            wordTrie.Insert(word);
        }
    }

    public void AddWord(string word)
    {
        stringsToType.Add(word.ToUpperInvariant());
        wordTrie.Insert(word);
    }

    public void RemoveWord(string word)
    {
        stringsToType.Remove(word.ToUpperInvariant());
        wordTrie.Remove(word);
    }

    public void WordFound(string word)
    {
        RemoveWord(word);
        PassWordToSpawner(word);
        // do other things like add score, spawn blocks etc.
    }

    private void PassWordToSpawner(string word)
    {
        Color colorTemp = colourChanger.ReturnRandomColor();

        char[] chars = word.Reverse().ToArray();

        foreach (char letter in chars)
        {
            NotifyObservers(letter, colorTemp);
        }
    }

    public bool CheckWord(string selectedWord)
    {
        if (wordTrie.Search(selectedWord))
        {
            Debug.Log("Word found in trie");
            return true;
        }
        else
        {
            Debug.Log("Word not found in trie");
            return false;
        }
    }

    public void AddObserver(IBlockObserver observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(IBlockObserver observer)
    {
        observers.Remove(observer);
    }

    public void NotifyObservers(char character, Color color)
    {
        foreach (IBlockObserver observer in observers)
        {
            observer.OnBlockSpawn(character, color);
        }
    }
}