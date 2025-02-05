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
        UIType uiType = UIElement.GetUIType();
        if (uiElements.ContainsKey(uiType))
        {
            Debug.LogWarning($"{UIElement.gameObject.name} reference already assigned. Overwriting reference.");
        }
        else
        {
            Debug.Log($"{UIElement.gameObject.name} is being assigned.");
        }
        uiElements[uiType] = UIElement;
    }

    public GameObject GetObjectInDictionary(UIType uiType)
    {
        if (uiElements.TryGetValue(uiType, out BaseUIObject uiObject))
        {
            return uiObject.gameObject;
        }
        else
        {
            Debug.LogWarning($"UI element of type {uiType} not found.");
            return null;
        }
    }
}