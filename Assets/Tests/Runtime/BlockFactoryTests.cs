using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using TMPro;
using UnityEditor;
using UnityEngine.TestTools;

public class BlockFactoryTests
{
    BlockTracker blockTracker;
    Material particleMaterial;

    BlockFactory blockFactory;
    GameObject prefab;


    [SetUp]
    public void SetUp()
    {
        prefab = new GameObject("TestPrefab");
        MeshRenderer meshRend = prefab.AddComponent<MeshRenderer>();

        GameObject letterTextObject = new GameObject("Letter Text");
        GameObject particleExplosionObject = new GameObject("Particle Explosion");
        letterTextObject.transform.parent = prefab.transform;
        particleExplosionObject.transform.parent = prefab.transform;

        ParticleSystemRenderer partRend = particleExplosionObject.AddComponent<ParticleSystemRenderer>();
        particleMaterial = partRend.material;
        letterTextObject.AddComponent<TextMeshPro>();

        GameObject factoryObject = new GameObject("BlockFactory");
        blockFactory = factoryObject.AddComponent<BlockFactory>();

        blockFactory.prefab = prefab;

        // dependencies
        blockTracker = new GameObject("BlockTracker").AddComponent<BlockTracker>();
        GameObject nextBlockPosPrefab = new GameObject("NextBlockPosition");
        nextBlockPosPrefab.transform.parent = prefab.transform;
    }

    [UnityTest]
    public IEnumerator CreateLetterBlockTest()
    {
        char character = 'A';
        Vector3 position = new Vector3(0, 0, 0);
        Color color = Color.red;

        GameObject block = blockFactory.CreateLetterBlock(character, position, color);

        // wait for one frame
        yield return null;

        Assert.IsNotNull(block, "Block should not be null");
        Assert.AreEqual(character.ToString(), block.GetComponentInChildren<TextMeshPro>().text, "Block text should match the character");
        Assert.AreEqual(color, block.GetComponent<MeshRenderer>().material.color, "Block color should match the specified color");
        Assert.AreEqual(color, particleMaterial.color, "Particle color should match the block color");
    }

    [TearDown]
    public void TearDown()
    {
        prefab = null;
        blockFactory = null;
        blockTracker = null;
    }
}
