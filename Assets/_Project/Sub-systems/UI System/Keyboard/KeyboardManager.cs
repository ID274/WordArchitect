using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(KeyboardListener))]
public class KeyboardManager : MonoBehaviour
{
    public static KeyboardManager Instance { get; private set; }

    [Header("Attributes")]
    public int maxInputLength = 18;
    public bool keyboardActive = false;

    [Header("References")]
    public GameObject keyboard;

    public string currentInput = "";
    public Dictionary<string, bool> permittedKeys = new Dictionary<string, bool>();
    public string[] permittedKeysArray;

    private CustomInputField inputField;

    private void Awake()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        InitializeKeyboard();
    }

    public void InitializeKeyboard()
    {
        if (keyboard != null)
        {
            foreach (var key in permittedKeysArray)
            {
                permittedKeys.Add(key, true);
            }
        }
        keyboard.SetActive(true);
        keyboardActive = true;

        StartCoroutine(AssignInputFieldDelay(0.1f));
    }

    private IEnumerator AssignInputFieldDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        inputField = UIReferenceManager.Instance.GetObjectInDictionary(UIType.customInputField).GetComponent<CustomInputField>();
        if (inputField == null)
        {
            Debug.LogError("Input field not found! Destroying keyboard.");
            Destroy(keyboard);
        }
    }

    public void OnBackspacePressed()
    {
        if (keyboard != null && keyboardActive == true)
        {
            if (currentInput.Length > 0)
            {
                currentInput = currentInput.Substring(0, currentInput.Length - 1);
                ValueChanged();
            }
        }
    }

    public bool TakeInput(char key)
    {
        key = char.ToUpperInvariant(key);
        if (keyboard != null && keyboardActive == true)
        {
            if (currentInput.Length < maxInputLength)
            {
                currentInput += key;
                ValueChanged();
                return true;
            }
        }
        return false;
    }

    public void DisableKeyboard()
    {
        keyboardActive = false;
        keyboard.SetActive(false);
    }

    public void EnableKeyboard()
    {
        keyboardActive = true;
        keyboard.SetActive(true);
    }

    public void ClearInput()
    {
        currentInput = "";
    }

    private void ValueChanged()
    {
        Debug.Log("Current input: " + currentInput);
        Debug.Log("Current input length: " + currentInput.Length);

        inputField.ClearInput();
        inputField.CallSound();

        foreach (char c in currentInput)
        {
            inputField.AddCharacter(c);
        }

        if (inputField.CheckCurrentWord())
        {
            ClearInput();
        }
    }
}
