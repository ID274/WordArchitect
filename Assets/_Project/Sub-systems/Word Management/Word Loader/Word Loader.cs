using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// This class allows for adding new text files via the inspector, therefore adhering to the Open/Closed Principle.

public class WordLoader : MonoBehaviour
{
    [Header("Text Assets (Drag and drop text files here)")]
    [SerializeField] private List<TextAsset> wordListFiles = new List<TextAsset>();

    private List<string> textFileNames = new List<string>();

    private IFileParser parserStrategy = new NewlineParser();

    private void Awake()
    {
        if (wordListFiles.Count == 0)
        {
            Debug.LogWarning("No text files have been assigned in the inspector or loaded.");
            return;
        }

        // Extract and log the file names
        foreach (var textAsset in wordListFiles)
        {
            if (textAsset != null)
            {
                string fileName = textAsset.name;
                textFileNames.Add(fileName);
                Debug.Log("Loaded file: " + fileName);
            }
        }
    }

    public void LoadParser(IFileParser parserStrategy)
    {
        this.parserStrategy = parserStrategy;
    }

    public string[] GetThemeNames()
    {
        if (textFileNames == null || textFileNames.Count == 0)
        {
            Debug.LogWarning("No text files found.");
            return new string[0];
        }

        return textFileNames.ToArray();
    }

    public void LoadWordList(int index, IFileParser parser)
    {
        if (index < 0 || index >= wordListFiles.Count)
        {
            Debug.LogError("Index out of range: " + index);
            return;
        }

        TextAsset selectedFile = wordListFiles[index];
        if (selectedFile == null)
        {
            Debug.LogError("The selected text file is null.");
            return;
        }

        string[] words = parser.GetWordsFromFile(selectedFile);
        ProcessWords(words, selectedFile.name);
    }

    private void ProcessWords(string[] words, string fileName)
    {
        List<string> wordsTemp = new List<string>();
        int count = 0;

        foreach (string word in words)
        {
            string trimmedWord = word.Trim();
            if (!string.IsNullOrWhiteSpace(trimmedWord))
            {
                Debug.Log(trimmedWord);
                WordHashSet.Instance.AddWord(trimmedWord);
                count++;
                wordsTemp.Add(trimmedWord);
            }
        }

        Debug.Log($"Number of words added to dictionary from {fileName}: {count}");
        ThemeHolder.Instance.SetTheme(fileName);
        WordSearchManager.Instance.PopulateTrie(wordsTemp.ToArray());
    }

    public void LoadRandomWordList()
    {
        if (wordListFiles.Count == 0)
        {
            Debug.LogError("No word lists available to load.");
            return;
        }

        int index = Random.Range(0, wordListFiles.Count);
        LoadWordList(index, parserStrategy);
    }
}