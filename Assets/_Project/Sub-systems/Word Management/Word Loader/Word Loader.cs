using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WordLoader : MonoBehaviour
{
    private TextAsset[] textFiles;
    private List<string> textFileNames = new List<string>();

    private string folderPath;

    private void Awake()
    {
        folderPath = "WordLists"; // The folder inside Resources
        int textFileCount = CountTextFiles(folderPath);
        Debug.Log("Number of text files: " + textFileCount);
    }

    private int CountTextFiles(string folderPath)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        StartCoroutine(CountTextFilesWebGL(folderPath));
        return textFileNames.Count;
#else
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
#endif
    }

    private IEnumerator CountTextFilesWebGL(string folderPath)
    {
        string path = Application.streamingAssetsPath + "/" + folderPath;
        UnityWebRequest request = UnityWebRequest.Get(path);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error accessing folder: " + request.error);
        }
        else
        {
            // Assuming the server returns a list of files in the directory
            string[] files = request.downloadHandler.text.Split('\n');
            foreach (string file in files)
            {
                if (file.EndsWith(".txt"))
                {
                    textFileNames.Add(file);
                }
            }
            Debug.Log("Number of text files: " + textFileNames.Count);
        }
    }

    public string[] GetThemeNames()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        return textFileNames.ToArray();
#else
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
#endif
    }

    private void LoadWordList(int index)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        StartCoroutine(LoadWordListWebGL(index));
#else
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
#endif
    }

    private IEnumerator LoadWordListWebGL(int index)
    {
        Debug.Log("Loading Word List by index");

        if (index < 0 || index >= textFileNames.Count)
        {
            Debug.LogError("Index out of range: " + index, this.gameObject);
            yield break;
        }

        string fileName = textFileNames[index];
        string path = Application.streamingAssetsPath + "/WordLists/" + fileName;
        UnityWebRequest request = UnityWebRequest.Get(path);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error loading file: " + request.error);
        }
        else
        {
            string[] words = request.downloadHandler.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
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
            ThemeHolder.Instance.SetTheme(fileName);
            WordSearchManager.Instance.PopulateTrie(wordsTemp.ToArray());
        }
    }

    public void LoadRandomWordList()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        if (textFileNames.Count == 0)
        {
            Debug.LogError("No word lists available to load.");
            return;
        }

        int index = Random.Range(0, textFileNames.Count);
        StartCoroutine(LoadWordListWebGL(index));
#else
        if (textFiles == null || textFiles.Length == 0)
        {
            Debug.LogError("No word lists available to load.");
            return;
        }

        int index = Random.Range(0, textFiles.Length);
        LoadWordList(index);
#endif
    }
}