using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFileParser
{
    string[] GetWordsFromFile(TextAsset textfile);
}
