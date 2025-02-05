using System.Collections.Generic;
using UnityEngine;

public class WordLoader : MonoBehaviour
{
    private TextAsset[] textFiles;

    private string folderPath;

    private void Awake()
    {
        folderPath = "WordLists"; // The folder inside Resources
        int textFileCount = CountTextFiles(folderPath);
        Debug.Log("Number of text files: " + textFileCount);
    }

    private int CountTextFiles(string folderPath)
    {
        // Load all TextAssets from the specified folder inside Resources
        textFiles = Resources.LoadAll<TextAsset>(folderPath);

        if (textFiles == null || textFiles.Length == 0)
        {
            Debug.LogError("No text files found in Resources/" + folderPath);
            return 0;
        }
        else
        {
            return textFiles.Length;
        }
    }

    public string[] GetThemeNames()
    {
        // Find every file in the directory and return the names of all files as a string array

        if (textFiles == null || textFiles.Length == 0)
        {
            Debug.LogWarning("No text files found in the directory.");
            return new string[0];
        }

        List<string> themeNames = new List<string>();
        foreach (var file in textFiles)
        {
            string fileName = file.name;
            themeNames.Add(fileName);
        }

        return themeNames.ToArray();
    }

    private void LoadWordList(int index)
    {
        Debug.Log("Loading Word List by index");

        if (index < 0 || index >= textFiles.Length)
        {
            Debug.LogError("Index out of range: " + index, this.gameObject);
            return;
        }

        TextAsset textFile = textFiles[index];
        string[] words = textFile.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
        Debug.Log("Number of words in text file: " + words.Length);

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

        Debug.Log("Number of words added to dictionary: " + count);
        ThemeHolder.Instance.SetTheme(textFile.name);
        WordSearchManager.Instance.PopulateTrie(wordsTemp.ToArray());
    }

    public void LoadRandomWordList()
    {
        if (textFiles == null || textFiles.Length == 0)
        {
            Debug.LogError("No word lists available to load.");
            return;
        }

        int index = Random.Range(0, textFiles.Length);
        LoadWordList(index);
    }
}