using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourChanger : MonoBehaviour
{
    [SerializeField] private Color[] allowedColors;

    private Color previousColor = Color.white;

    public Color ReturnRandomColor()
    {
        Color newColor;

        for (int i = 0; i < 20; i++)
        {
            newColor = allowedColors[Random.Range(0, allowedColors.Length)];

            if (newColor != previousColor)
            {
                previousColor = newColor;
                return newColor;
            }
        }
        Debug.LogWarning("ColourChanger: Could not find a new color, returning previous color.");
        return previousColor;
    }
}