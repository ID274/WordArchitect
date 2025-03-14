using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardListener : MonoBehaviour
{
    private void Awake()
    {
        // Disable script for platforms where keyboard input is not appropriate
        if (Application.isConsolePlatform || Application.isMobilePlatform)
        {
            enabled = false;
            return;
        }
    }

    private void Update()
    {
        // Handle backspace key
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            KeyboardManager.Instance.OnBackspacePressed();
        }

        // Check for valid key presses
        KeyCode key = GetKeyInputFromKeyboard();
        if (key != KeyCode.None) // Return None if no valid key is pressed
        {
            Debug.Log("Key pressed: " + key);
            KeyboardManager.Instance.TakeInput((char)key);
        }
    }

    private bool CheckKeyIsValid(string key)
    {
        key = key.ToUpperInvariant();

        // Check if the key is a letter on the QWERTY English keyboard
        return key.Length == 1 && key[0] >= 'A' && key[0] <= 'Z';
    }

    private KeyCode GetKeyInputFromKeyboard()
    {
        foreach (KeyCode vkey in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(vkey)) // Only trigger on key down
            {
                if (CheckKeyIsValid(vkey.ToString()))
                {
                    return vkey;
                }
            }
        }
        return KeyCode.None; // Return None when no valid key is pressed
    }
}