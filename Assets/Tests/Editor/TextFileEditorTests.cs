using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using System.IO;
using UnityEditor;

public class TextFileEditorTests
{
    TextFileEditor textFileEditor;
    TextAsset textAsset;
    string testFilePath = "Assets/Tests/Editor/TextFileEditorTests.txt";

    [SetUp]
    public void SetUp()
    {
        textFileEditor = (TextFileEditor)ScriptableObject.CreateInstance(typeof(TextFileEditor));

        // reset the TextFileEditor just in case
        textFileEditor.SetDefaultValues(true);

        textFileEditor.filePath = testFilePath;

        File.WriteAllText(testFilePath, ""); // create an empty file

        AssetDatabase.Refresh();

        // load the file as a TextAsset
        textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(testFilePath);

        Assert.IsNotNull(textAsset, "TextAsset should not be null");
    }

    [Test]
    public void PropertyLowerCaseSetterTest()
    {
        textFileEditor.WordA = "test";
        textFileEditor.WordB = "TEST";
        Assert.AreEqual("test", textFileEditor.WordA);
        Assert.AreEqual("test", textFileEditor.WordB);
    }

    [Test]
    public void AddWordTest()
    {
        Assert.AreEqual(textAsset.text, "");
        textFileEditor.WordA = "test1";
        textFileEditor.AddWord(false, textFileEditor.WordA, textAsset);
        Assert.IsTrue(textAsset.text.Contains("test1"));
    }

    [Test]
    public void AddWordDuplicateTest()
    {
        Assert.AreEqual(textAsset.text, "");
        textFileEditor.WordA = "test1";
        textFileEditor.AddWord(false, textFileEditor.WordA, textAsset);
        textFileEditor.AddWord(false, textFileEditor.WordA, textAsset);
        Assert.IsTrue(textAsset.text.Contains("test1"));

        // checks for duplication
        string[] strings = textAsset.text.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        Assert.AreEqual(1, strings.Length);
    }

    [Test]
    public void ReplaceWordTest()
    {
        Assert.AreEqual(textAsset.text, "");
        textFileEditor.WordA = "test1";
        textFileEditor.AddWord(false, textFileEditor.WordA, textAsset);
        textFileEditor.WordB = "test2";
        textFileEditor.ReplaceWord(false, textFileEditor.WordA, textFileEditor.WordB, textAsset);
        Assert.IsFalse(textAsset.text.Contains("test1"));
        Assert.IsTrue(textAsset.text.Contains("test2"));
    }

    [Test]
    public void ReplaceWordDuplicateTest()
    {
        Assert.AreEqual(textAsset.text, "");
        textFileEditor.WordA = "test1";
        textFileEditor.AddWord(false, textFileEditor.WordA, textAsset);
        textFileEditor.AddWord(false, "test2", textAsset);
        textFileEditor.WordB = "test2";
        textFileEditor.ReplaceWord(false, textFileEditor.WordA, textFileEditor.WordB, textAsset);
        Assert.IsTrue(textAsset.text.Contains("test1"));
        Assert.IsTrue(textAsset.text.Contains("test2"));

        // checks for duplication
        Debug.Log("TextAsset content: " + textAsset.text);
        string[] strings = textAsset.text.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        Assert.AreEqual(2, strings.Length);
    }

    [Test]
    public void RemoveWordTest()
    {
        Assert.AreEqual(textAsset.text, "");
        string[] words = { "test1", "test2", "test3" };
        foreach (string word in words)
        {
            textFileEditor.AddWord(false, word, textAsset);
        }
        Assert.AreEqual(words.Length, textAsset.text.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries).Length);

        textFileEditor.RemoveWord("test1", textAsset);
        Assert.IsFalse(textAsset.text.Contains("test1"));
        Assert.IsTrue(textAsset.text.Contains("test2"));
        Assert.IsTrue(textAsset.text.Contains("test3"));

        Debug.Log($"TextAsset content:\n{textAsset.text}");
        Assert.AreEqual(words.Length - 1, textAsset.text.Split(new[] { '\n' }).Length);
    }

    [Test]
    public void SortAlphabeticallyTest()
    {
        Assert.AreEqual(textAsset.text, "");
        string[] words = { "C", "A", "B" };
        foreach (string word in words)
        {
            textFileEditor.AddWord(false, word, textAsset);
        }
        Assert.AreEqual(words.Length, textAsset.text.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries).Length);

        Debug.Log($"TextAsset content before sorting:\n{textAsset.text}");
        textFileEditor.SortAlphabetically(textAsset);
        Debug.Log($"TextAsset content after sorting:\n{textAsset.text}");

        string[] sortedWords = textAsset.text.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        Assert.AreEqual("a", sortedWords[0]);
        Assert.AreEqual("b", sortedWords[1]);
        Assert.AreEqual("c", sortedWords[2]);
    }

    [TearDown]
    public void TearDown()
    {
        textFileEditor = null;

        // clean up temporary file including the meta file
        File.Delete(AssetDatabase.GetAssetPath(textAsset));
        textAsset = null;

        Assert.IsFalse(File.Exists(testFilePath), "Test file should be deleted");
        AssetDatabase.Refresh();
    }
}
