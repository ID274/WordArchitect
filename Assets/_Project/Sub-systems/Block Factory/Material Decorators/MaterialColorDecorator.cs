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
    public void ApplyColor(Material newMat, Material oldMat)
    {
        if (CanApplyColor(newMat))
        {
            newMat.SetColor(colorProperty, oldMat.color);
        }
        else
        {
            Debug.LogWarning("Shader does not have a _Color or _TintColor property.", newMat);
        }
    }
}
