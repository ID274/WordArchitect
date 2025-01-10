using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Backspace : MonoBehaviour
{
    private Button button;
    private KeyAnimation keyAnimation;

    private void Awake()
    {
        button = GetComponent<Button>();
        keyAnimation = GetComponent<KeyAnimation>();
        button.onClick.AddListener(BackspacePressed);
    }

    private void BackspacePressed()
    {
        KeyboardManager.Instance.OnBackspacePressed();

        if (keyAnimation != null)
        {
            keyAnimation.PlayAnimation();
        }
    }
}
