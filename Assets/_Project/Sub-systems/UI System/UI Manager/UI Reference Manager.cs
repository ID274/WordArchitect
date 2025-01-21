using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIReferenceManager : MonoBehaviour
{
    public static UIReferenceManager Instance { get; private set; }

    [Header("UI References")]
    public BaseUIObject keyboard;
    public BaseUIObject customInputField;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple instances of UIReferenceManager found. Destroying the new instance.");
            Destroy(this);
        }
    }

    public void AssignMe(BaseUIObject UIElement)
    {
        // Here we can assign a UI element to its corresponding reference in the UIReferenceManager, allowing us to have a central place to access all UI elements.
        // The UI Reference Manager does not care what the actual UI element is, as long as it implements the IUIElement interface. It then uses that interface to
        // determine what type of UI element it is, and assigns it to the appropriate reference. It doesn't care what is put into it, 

        switch (UIElement.GetUIType())
        {
            case UIType.keyboard:
                if (keyboard != null)
                {
                    Debug.LogWarning("Keyboard reference already assigned. Overwriting reference.");
                }
                else
                {
                    Debug.Log("Keyboard has been assigned.");
                }
                keyboard = UIElement;
                break;
            case UIType.customInputField:
                if (customInputField != null)
                {
                    Debug.LogWarning("CustomInputField reference already assigned. Overwriting reference.");
                }
                else
                {
                    Debug.Log("Custom Input Field has been assigned.");
                }
                customInputField = UIElement;
                break;
            default:
                Debug.LogError("Invalid UIType");
                break;
        }
    }
}
