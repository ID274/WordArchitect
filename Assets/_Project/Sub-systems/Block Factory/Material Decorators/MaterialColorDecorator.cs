using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialColorDecorator : IMaterialColorDecorator
{
    private string colorProperty = "_Color";
    public bool CanApplyColor(Material material)
    {
        if (material.HasProperty("_Color") || material.HasProperty("_TintColor"))
        {
            colorProperty = material.HasProperty("_Color") ? "_Color" : "_TintColor";
            return true;
        }

        return false;
    }
    public void ApplyColor(Material materialToChange, Material materialToUse)
    {
        if (CanApplyColor(materialToChange))
        {
            materialToChange.SetColor(colorProperty, materialToUse.color);
            Debug.Log($"Material color changed to {materialToUse.color}", materialToChange);
        }
        else
        {
            Debug.LogWarning("Shader does not have a _Color or _TintColor property.", materialToChange);
        }
    }
}
