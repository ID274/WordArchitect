using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMaterialColorDecorator
{
    bool CanApplyColor(Material material);
    void ApplyColor(Material newMat, Material oldMat);
}
