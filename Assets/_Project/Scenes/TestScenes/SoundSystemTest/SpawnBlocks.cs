using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBlocks : MonoBehaviour
{
    public GameObject blockPrefab;

    public int blocksSpawned = 0;

    public float timeBetweenSpawns = 1f;
    private float timeElapsed = 0f;

    private void Update()
    {
        if (timeElapsed >= timeBetweenSpawns)
        {
            GameObject block = Instantiate(blockPrefab, new Vector3(blocksSpawned, 0, 0), Quaternion.identity);
            block.name = $"Block {blocksSpawned}";
            blocksSpawned++;
            timeElapsed = 0f;
        }
        else
        {
            timeElapsed += Time.deltaTime;
        }
    }
}
