using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIReferenceManager : MonoBehaviour
{
    public static UIReferenceManager Instance { get; private set; }

    public CustomInputField customInputField;

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

    public void ReferenceMe(GameObject reference)
    {
        if (reference.TryGetComponent(out CustomInputField tempInputField))
        {
            customInputField = tempInputField;
        }
        // can be extended to other UI elements
    }
}
