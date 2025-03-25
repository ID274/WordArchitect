using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

#if UNITY_EDITOR
public class MockBlockObserver : MonoBehaviour, IBlockObserver
{
    public char character {  get; private set; }
    public Color color { get; private set; }

    public IBlockObserverSubject GrabBlockObserverSubject()
    {
        IBlockObserverSubject baseObserverSubject = FindObjectOfType<BaseObserverSubject<IBlockObserver>>() as IBlockObserverSubject;

        if (baseObserverSubject != null) { Debug.LogError("IBlockObserverSubject not found."); }
        return baseObserverSubject;
    }

    public void OnBlockSpawn(char character, Color color)
    {
        Debug.Log($"OnBlockSpawn successful with: character = {character} and color = {color}");
        this.color = color;
        this.character = character;
    }
}

#endif