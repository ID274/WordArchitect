using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeHolder : MonoBehaviour
{
    public static ThemeHolder Instance { get; private set; }

    private string themeName;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void SetTheme(string themeName)
    {
        this.themeName = themeName;
        Debug.Log("Theme set: " + themeName);
    }
}
