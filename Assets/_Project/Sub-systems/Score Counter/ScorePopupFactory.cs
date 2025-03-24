using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScorePopupFactory : MonoBehaviour
{
    [SerializeField] private Canvas targetCanvas;
    [SerializeField] private GameObject scorePopupPrefab;

    [SerializeField] private float minX, maxX, minZRotation, maxZRotation;

    private float lastXPosition = -1;
    public void CreateScorePopup(int score) // creates a score popup with the given score
    {
        string prefix = score >= 0 ? "+" : "-";
        string text = $"{prefix}{score}";

        // set text
        TextMeshProUGUI scorePopup = Instantiate(scorePopupPrefab, targetCanvas.transform).GetComponent<TextMeshProUGUI>();
        scorePopup.text = text;

        // set position and rotation
        float newXPosition;
        if (lastXPosition >= 0)
        {
            newXPosition = Random.Range(minX, 0);
        }
        else
        {
            newXPosition = Random.Range(0, maxX);
        }
        Vector3 position = new Vector3(newXPosition, scorePopupPrefab.transform.position.y, scorePopupPrefab.transform.position.z);
        lastXPosition = newXPosition;
        float zRotation = Random.Range(minZRotation, maxZRotation);
        scorePopup.transform.localPosition = position;
        scorePopup.transform.rotation = Quaternion.Euler(0, 0, zRotation);

        // set destroy delay based on animation length
        float length = 1;
        if (scorePopup.TryGetComponent(out Animation animation))
        {
            length = animation.clip.length;
        }
        StartCoroutine(DestroyAfterDelay(scorePopup.gameObject, length));
    }

    private IEnumerator DestroyAfterDelay(GameObject destroyObject, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(destroyObject);
    }
}
