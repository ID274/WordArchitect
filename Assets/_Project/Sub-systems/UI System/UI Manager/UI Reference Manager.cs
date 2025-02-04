using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIReferenceManager : MonoBehaviour
{
    public static UIReferenceManager Instance { get; private set; }
    public Dictionary<UIType, BaseUIObject> uiElements = new Dictionary<UIType, BaseUIObject>();

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
        // Here we can assign a UI element to its corresponding reference, allowing us to have a central place to access all UI elements.
        // The UI Reference Manager does not care what the actual UI element is, as long as it is of type BaseUIObject. It can then use the UIType enumerator
        // to determine which specific UI object it is so it can be assigned to its rightful place. This demonstrates use of Liskov's Substitution Principle.

        // Current approach unfortunately breaks O/P Principle

        if (uiElements.TryGetValue(UIElement.GetUIType(), out BaseUIObject uiObject))
        {
            Debug.LogWarning($"{UIElement.gameObject.name} reference already assigned. Overwriting reference.");
            uiElements.Add(UIElement.GetUIType(), uiObject);
        }
        else
        {
            Debug.Log($"{UIElement.gameObject.name} is being assigned.");
            uiElements.Add(UIElement.GetUIType(), uiObject);
        }
    }

    public GameObject GetObjectInDictionary(UIType uiType)
    {
        if (uiElements.TryGetValue(uiType, out BaseUIObject uiObject)) 
        {
            return uiObject.gameObject;
        }
        else
        {
            return null;
        }
    }
}