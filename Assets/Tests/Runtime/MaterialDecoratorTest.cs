using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;

public class MaterialColorDecoratorTests
{
    MaterialColorDecorator materialDecorator;
    Material materialA;
    Material materialB;

    [SetUp]
    public void Setup()
    {
        materialA = new Material(Shader.Find("Standard"));
        materialB = new Material(Shader.Find("Standard"));
        materialDecorator = new MaterialColorDecorator();
    }

    [UnityTest]
    public IEnumerator TestMaterialColorDecorator()
    {
        materialA.color = Color.red;
        materialB.color = Color.blue;

        materialDecorator.ApplyColor(materialA, materialB);

        yield return null;
        Assert.AreEqual(materialA.color, materialB.color);
        Assert.AreEqual(materialA.color, Color.blue);
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(materialA);
        Object.Destroy(materialB);
    }
}
