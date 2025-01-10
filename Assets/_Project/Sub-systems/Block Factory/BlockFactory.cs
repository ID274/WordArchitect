using TMPro;
using UnityEngine;

public class BlockFactory : MonoBehaviour
{
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
        // Check if the shader supports color changes
        if (newMat.HasProperty("_Color"))
        {
            newMat.SetColor("_Color", oldMat.color);
        }
        else if (newMat.HasProperty("_TintColor")) // Alternative common property
        {
            newMat.SetColor("_TintColor", oldMat.color);
        }
        else
        {
            Debug.LogWarning("Shader does not have a color or tint property.");
        }
    }
}
