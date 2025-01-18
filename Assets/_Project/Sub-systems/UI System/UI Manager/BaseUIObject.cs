using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUIObject : MonoBehaviour, IUIElement
{
    // base UI class because making SOLID makes my brain LIQUID

    [SerializeField] private UIType uiType;

    protected virtual void Awake()
    {
        InitializeThis();
    }

    protected void InitializeThis()
    {
        UIReferenceManager.Instance.AssignMe(this);
    }

    public UIType GetUIType()
    {
        return UIType.customInputField;
    }
}