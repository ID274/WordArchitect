using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ColourChanger))]
public class WordSearchManager : MonoBehaviour
{
    public static WordSearchManager Instance { get; private set; }

    private Trie wordTrie;
    private ColourChanger colourChanger;
    private List<string> stringsToType = new List<string>();

    private BlockSpawner blockSpawner;

    private void Awake()
    {
        blockSpawner = FindAnyObjectByType<BlockSpawner>();

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
        WordLoader wordLoader = gameObject.AddComponent(typeof(WordLoader)) as WordLoader;
        wordLoader.LoadRandomWordList();
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
            blockSpawner.AddBlockToSpawn(letter, colorTemp);
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
}