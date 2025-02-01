using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WordLoader : MonoBehaviour
{
    private string[] textFiles;

    private void Awake()
    {
        string folderPath = Application.streamingAssetsPath + "/WordLists";
        int textFileCount = CountTextFiles(folderPath);
        Debug.Log("Number of text files: " + textFileCount);
    }

    private int CountTextFiles(string folderPath)
    {
        if (Directory.Exists(folderPath))
        {
            textFiles = Directory.GetFiles(folderPath, "*.txt");
            return textFiles.Length;
        }
        else
        {
            Debug.LogError("Folder does not exist: " + folderPath);
            return 0;
        }
    }

    private void LoadWordList(int index)
    {
        Debug.Log("Loading Word List by index");

        if (index < 0 || index >= textFiles.Length)
        {
            Debug.LogError("Index out of range: " + index, this.gameObject);
            return;
        }

        string textFilePath = textFiles[index];
        string[] words = File.ReadAllLines(textFilePath);
        Debug.Log("Number of words in text file: " + words.Length);

        List<string> wordsTemp = new List<string>();

        int count = 0;
        foreach (string word in words)
        {
            if (!string.IsNullOrWhiteSpace(word))
            {
                Debug.Log(word);
                WordHashSet.Instance.AddWord(word);
                count++;
                wordsTemp.Add(word);
            }
        }

        Debug.Log("Number of words added to dictionary: " + count);
        ThemeHolder.Instance.SetTheme(Path.GetFileNameWithoutExtension(textFilePath));
        WordSearchManager.Instance.PopulateTrie(wordsTemp.ToArray());
    }

    private void LoadWordList(string fileName)
    {
        Debug.Log("Loading Word List by filename");

        string folderPath = Application.streamingAssetsPath + "/WordLists";
        string textFilePath = folderPath + "/" + fileName + ".txt";

        List<string> wordsTemp = new List<string>();

        if (File.Exists(textFilePath))
        {
            string[] words = File.ReadAllLines(textFilePath);
            Debug.Log("Number of words in text file: " + words.Length);

            int count = 0;
            foreach (string word in words)
            {
                if (!string.IsNullOrWhiteSpace(word))
                {
                    Debug.Log(word);
                    WordHashSet.Instance.AddWord(word);
                    count++;
                }
            }

            Debug.Log("Number of words added to dictionary: " + count);
        }
        else
        {
            Debug.LogError("File does not exist: " + textFilePath);
        }
        ThemeHolder.Instance.SetTheme(Path.GetFileNameWithoutExtension(textFilePath));
        WordSearchManager.Instance.PopulateTrie(wordsTemp.ToArray());
    }

    public void LoadWordListByIndex(int index)
    {
        LoadWordList(index);
    }

    public void LoadWordListByName(string fileName)
    {
        LoadWordList(fileName);
    }

    public void LoadRandomWordList()
    {
        int index = Random.Range(0, textFiles.Length);
        LoadWordList(index);
    }
}