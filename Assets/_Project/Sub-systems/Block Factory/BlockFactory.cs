using TMPro;
using UnityEngine;
using System.Collections;

public class BlockFactory : MonoBehaviour
{
    private IMaterialColorDecorator materialColorDecorator; // Using a decorator pattern to separate color logic from the factory

    private void Awake()
    {
        materialColorDecorator = new MaterialColorDecorator();
    }
    public GameObject CreateLetterBlock(GameObject prefab, char character, Vector3 position, Color color)
    {
        if (prefab == null || character == ' ')
        {
            return null;
        }

        GameObject element = Instantiate(prefab, position, Quaternion.identity);
        element.GetComponentInChildren<TextMeshPro>().text = character.ToString();

        if (element != null)
        {
            BlockTracker.Instance.AddBlock(element);
        }
        else
        {
            Debug.LogError("Created block is null");
        }

        Material tempMat = element.GetComponent<MeshRenderer>().material;
        tempMat.color = color;
        element.GetComponent<MeshRenderer>().material = tempMat;

        ParticleSystemRenderer particleSystemRenderer = element.GetComponentInChildren<ParticleSystemRenderer>();
        // Get the current material instance
        Material particleMat = particleSystemRenderer.material;
        // Set the particle color to the same as the block
        SetParticleColor(particleMat, tempMat);

        return element;
    }

    private void SetParticleColor(Material newMat, Material oldMat)
    {
        if (materialColorDecorator == null)
        {
            Debug.LogWarning("Material color decorator is null.");
            return;
        }

        if (newMat == null || oldMat == null)
        {
            Debug.LogWarning("Material is null");
            return;
        }
        materialColorDecorator.ApplyColor(newMat, oldMat);
    }
}
