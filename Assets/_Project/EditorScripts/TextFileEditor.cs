using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

public enum TextFileEditorState
{
    None = 0, // default to None
    Add,
    Replace,
    Remove
}

public class TextFileEditor : EditorWindow
{
    private TextFileEditorState state = TextFileEditorState.None;
    private string wordA = "";
    public string WordA
    {
        get
        {
            return wordA;
        }
        internal set
        {
            wordA = value;
            wordA = wordA.ToLower(); // lowercase the word when setting
        }
    }
    private string wordB = "";
    public string WordB
    {
        get
        {
            return wordB;
        }
        internal set
        {
            wordB = value;
            wordB = wordB.ToLower(); // lowercase the word when setting
        }
    }

    private TextAsset wordFile;
    internal string filePath;
    [Tooltip("Allow duplicate instances of the word?")] private bool allowDuplicates = false;

    [MenuItem("Window/TextFileEditor")]
    public static void ShowWindow()
    {
        GetWindow<TextFileEditor>("TextFileEditor");
    }

    private void OnEnable()
    {
        Debug.Log($"TextFileEditor enabled.");
    }

    private void OnDisable()
    {
        SetDefaultValues(true);
        Debug.Log($"TextFileEditor disabled.");
    }

    internal void SetDefaultValues(bool includeFile) // resets to "default" state
    {
        wordA = "";
        wordB = "";
        if (includeFile)
        {
            wordFile = null;
            filePath = "";
        }
        allowDuplicates = false;
        state = TextFileEditorState.None;
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("TextFileEditor", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        if (GUILayout.Button("Select file"))
        {
            filePath = EditorUtility.OpenFilePanel("Select text file", "", "txt");
            if (!string.IsNullOrEmpty(filePath))
            {
                SetDefaultValues(false);
                Debug.Log("File path: " + filePath);
                string relativePath = "Assets" + filePath.Substring(Application.dataPath.Length);
                wordFile = AssetDatabase.LoadAssetAtPath<TextAsset>(relativePath);

                if (wordFile != null)
                {
                    Debug.Log("Successfully loaded TextAsset.");
                }
                else
                {
                    Debug.LogError("Failed to load TextAsset. Ensure the file is within the Assets folder.");
                }
            }
        }

        if (wordFile != null) // check if file is selected
        {
            EditorGUILayout.LabelField("File loaded: " + wordFile.name);
            allowDuplicates = EditorGUILayout.Toggle("Allow duplicates", allowDuplicates);

            EditorGUILayout.Space(10);
            if (GUILayout.Button("Add") || state == TextFileEditorState.Add)
            {
                state = TextFileEditorState.Add;
                WordA = EditorGUILayout.TextField("Word to add", wordA);
                if (!string.IsNullOrEmpty(wordA) && GUILayout.Button("Confirm"))
                {
                    AddWord(allowDuplicates, wordA, wordFile);
                }
            }
            EditorGUILayout.Space(10);
            if (GUILayout.Button("Replace") || state == TextFileEditorState.Replace)
            {
                state = TextFileEditorState.Replace;
                WordA = EditorGUILayout.TextField("Word to replace", wordA);
                WordB = EditorGUILayout.TextField("Word to replace with", wordB);
                if (!string.IsNullOrEmpty(wordA) && !string.IsNullOrEmpty(wordB) && GUILayout.Button("Confirm"))
                {
                    ReplaceWord(allowDuplicates, wordA, wordB, wordFile);
                }
            }
            EditorGUILayout.Space(10);
            if (GUILayout.Button("Remove") || state == TextFileEditorState.Remove)
            {
                state = TextFileEditorState.Remove;
                WordA = EditorGUILayout.TextField("Word to remove", wordA);
                if (!string.IsNullOrEmpty(wordA) && GUILayout.Button("Confirm"))
                {
                    RemoveWord(wordA, wordFile);
                }
            }

            EditorGUILayout.Space(30);
            if (GUILayout.Button("Sort alphabetically")) // doesnt need state, can sort whenever
            {
                SortAlphabetically(wordFile);
            }
        }
    }


    // internal method to expose it for unit testing
    internal void AddWord(bool allowDuplicates, string wordToAdd, TextAsset file)
    {
        if (string.IsNullOrEmpty(wordToAdd) || file == null)
        {
            Debug.LogError("Word or file is null or empty.");
            return;
        }
        
        wordToAdd = wordToAdd.ToLower();

        string newText = file.text + "\n" + wordToAdd;
        if (allowDuplicates)
        {
            File.WriteAllText(AssetDatabase.GetAssetPath(file), newText);
            Debug.Log($"Word {wordToAdd} added to the file.");
            return;
        }

        if (!file.text.Contains(wordToAdd))
        {
            File.WriteAllText(AssetDatabase.GetAssetPath(file), newText);
            Debug.Log($"Word {wordToAdd} added to the file.");
        }
        else
        {
            Debug.LogWarning($"Word {wordToAdd} already exists in the file and duplicates are disabled.");
        }

        // refresh the asset database to reflect changes
        AssetDatabase.Refresh();
    }

    internal void ReplaceWord(bool allowDuplicates, string wordToReplace, string wordToReplaceWith, TextAsset file)
    {
        if (string.IsNullOrEmpty(wordToReplace) || string.IsNullOrEmpty(wordToReplaceWith) || file == null)
        {
            Debug.LogError("Word or file is null or empty.");
            return;
        }

        if (allowDuplicates)
        {
            string newText = file.text.Replace(wordToReplace, wordToReplaceWith);
            File.WriteAllText(AssetDatabase.GetAssetPath(file), newText);
            Debug.Log($"Word {wordToReplace} replaced with {wordToReplaceWith} in the file.");
            return;
        }

        if (!file.text.Contains(wordToReplaceWith))
        {
            string newText = file.text.Replace(wordToReplace, wordToReplaceWith);
            File.WriteAllText(AssetDatabase.GetAssetPath(file), newText);
            Debug.Log($"Word {wordToReplace} replaced with {wordToReplaceWith} in the file.");
        }
        else
        {
            Debug.LogWarning($"Word {wordToReplaceWith} already exists in the file and duplicates are disabled.");
        }

        // refresh the asset database to reflect changes
        AssetDatabase.Refresh();
    }

    internal void RemoveWord(string wordToRemove, TextAsset file)
    {
        if (string.IsNullOrEmpty(wordToRemove) || file == null)
        {
            Debug.LogError("Word or file is null or empty.");
            return;
        }

        string[] lines = file.text.Split('\n');
        string newText = string.Join("\n", lines.Where(line => !line.Contains(wordToRemove)));
        newText = string.Join("\n", newText.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries));
        File.WriteAllText(AssetDatabase.GetAssetPath(file), newText);
        Debug.Log($"Word {wordToRemove} removed from the file.");

        // refresh the asset database to reflect changes
        AssetDatabase.Refresh();
    }

    internal void SortAlphabetically(TextAsset textFile)
    {
        wordFile = textFile;

        if (wordFile == null)
        {
            Debug.LogError("File is null.");
            return;
        }

        string[] lines = wordFile.text.Split('\n');
        Array.Sort(lines);
        string newText = string.Join("\n", lines);
        File.WriteAllText(AssetDatabase.GetAssetPath(wordFile), newText);
        Debug.Log("File sorted alphabetically.");

        // refresh the asset database to reflect changes
        AssetDatabase.Refresh();
    }
}

#endif