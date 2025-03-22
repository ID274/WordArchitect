using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMaterialColorDecorator
{
    bool CanApplyColor(Material material); // validation to make sure the material can be colored
    void ApplyColor(Material newMat, Material oldMat); // actual application of the color to the material
}
