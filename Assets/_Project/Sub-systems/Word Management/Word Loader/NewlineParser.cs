using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewlineParser : IFileParser
{
    public string[] GetWordsFromFile(TextAsset textfile)
    {
        if (textfile == null || string.IsNullOrEmpty(textfile.text)) 
        {
            Debug.LogError($"TextAsset {textfile} is null or empty.");
            return null;
        }

        string[] words = textfile.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

        return words;
    }
}
