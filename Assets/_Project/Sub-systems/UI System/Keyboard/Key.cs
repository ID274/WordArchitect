using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class Key : MonoBehaviour, IKey
{
    [SerializeField] private char key;

    private KeyAnimation keyAnimation;

    private void Awake()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonPressed);
        GetComponentInChildren<TextMeshProUGUI>().text = key.ToString();
        gameObject.name = $"Key {key.ToString()}";

        keyAnimation = GetComponent<KeyAnimation>();
    }
    public void OnButtonPressed()
    {
        OnKeyPressed(key);
    }
    public void OnKeyPressed(char key)
    {
        if (KeyboardManager.Instance.TakeInput(key))
        {
            // play key sfx
            // play key animation

            if (keyAnimation != null)
            {
                keyAnimation.PlayAnimation();
            }
        }
    }
}
