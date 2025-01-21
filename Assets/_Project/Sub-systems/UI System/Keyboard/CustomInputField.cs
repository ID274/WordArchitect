using TMPro;
using UnityEngine;

public class CustomInputField : BaseUIObject
{
    private TextMeshProUGUI text;
    private string currentInput;

    protected void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        if (text == null)
        {
            Debug.LogError("TextMeshProUGUI component not found!", this.gameObject);
            return;
        }
        currentInput = "";
    }

    protected override void Start()
    {
        base.Start();
    }

    // SHOULD MAYBE DECOUPLE THE BELOW CODE INTO A SEPARATE INPUT FIELD MANAGER TO FOLLOW SRP

    public void AddCharacter(char character)
    {
        currentInput += character;
        text.text = currentInput;
    }

    public void RemoveCharacter()
    {
        if (currentInput.Length > 0)
        {
            currentInput = currentInput.Remove(currentInput.Length - 1);
            text.text = currentInput;
        }
    }

    public void ClearInput()
    {
        currentInput = "";
        text.text = currentInput;
    }

    public bool CheckCurrentWord()
    {
        bool wordFound = WordSearchManager.Instance.CheckWord(currentInput);

        if (wordFound)
        {
            WordSearchManager.Instance.WordFound(currentInput);
            ClearInput();
            return true;
        }
        else
        {
            Debug.Log("Word not found: " + currentInput);
            return false;
        }
    }
}
