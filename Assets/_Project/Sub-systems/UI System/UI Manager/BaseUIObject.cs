using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUIObject : MonoBehaviour
{
    // base UI class because making SOLID makes my brain LIQUID

    [SerializeField] protected UIType uiType;

    protected virtual void Start()
    {
        InitializeThis();
    }

    protected void InitializeThis()
    {
        UIReferenceManager.Instance.AssignMe(this);
    }

    public UIType GetUIType()
    {
        return uiType; ; // here we tell the UI reference manager what type of UI it is, so it knows where to assign it
    }
}