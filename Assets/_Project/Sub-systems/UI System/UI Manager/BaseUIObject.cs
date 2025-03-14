using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseUIObject : MonoBehaviour
{
    [SerializeField] protected UIType uiType;

    private float delay = 0.1f;

    protected virtual void Awake()
    {
        StartCoroutine(DelayInitialization(delay));
    }

    protected void InitializeThis()
    {
        UIReferenceManager.Instance.AssignMe(this);
    }

    private IEnumerator DelayInitialization(float delay)
    {
        yield return new WaitForSeconds(delay);
        InitializeThis();
    }

    public UIType GetUIType()
    {
        return uiType; ; // here we tell the UI reference manager what type of UI it is, so it knows where to assign it
    }
}