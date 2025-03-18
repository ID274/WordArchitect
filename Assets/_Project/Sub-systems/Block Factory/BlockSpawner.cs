using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BlockFactory))]
public class BlockSpawner : MonoBehaviour
{
    [SerializeField] private BlockFactory blockFactory;

    [SerializeField] private GameObject prefabDefault;

    Queue<(GameObject, char, Vector3, Color)> blocksToSpawn = new Queue<(GameObject, char, Vector3, Color)>();

    [SerializeField] private float spawnInterval = 0.5f;
    private bool onCooldown = false;

    public static Action onBlockSpawn;

    private void Awake()
    {
        blockFactory = GetComponent<BlockFactory>();
    }

    private void FixedUpdate()
    {
        if (blocksToSpawn.Count > 0 && !onCooldown)
        {
            (GameObject, char, Vector3, Color) blockData = blocksToSpawn.Dequeue();
            SpawnBlock(blockData.Item1, blockData.Item2, blockData.Item4);
            onBlockSpawn?.Invoke();
        }
    }

    private void OnEnable()
    {
        WordSearchManager.spawnBlock += AddBlockToSpawn;
    }

    private void OnDisable()
    {
        WordSearchManager.spawnBlock -= AddBlockToSpawn;
    }

    private IEnumerator SpawnBlockCoroutine()
    {
        onCooldown = true;
        yield return new WaitForSeconds(spawnInterval);
        onCooldown = false;
    }

    private void SpawnBlock(GameObject prefab, char character, Color color)
    {
        SpawnBlock(prefab, character, BlockTracker.Instance.nextBlockPosition, color);
    }

    private void SpawnBlock(GameObject prefab, char character, Vector3 position, Color color)
    {
        GameObject newBlock = blockFactory.CreateLetterBlock(prefab, character, position, color);
        StartCoroutine(SpawnBlockCoroutine());
    }

    public void AddBlockToSpawn(char character, Color color)
    {
        AddBlockToSpawn(prefabDefault, character, BlockTracker.Instance.nextBlockPosition, color);
    }

    public void AddBlockToSpawn(GameObject prefab, char character, Color color)
    {
        AddBlockToSpawn(prefab, character, BlockTracker.Instance.nextBlockPosition, color);
    }

    private void AddBlockToSpawn(GameObject prefab, char character, Vector3 position, Color color)
    {
        (GameObject, char, Vector3, Color) newBlockData = (prefab, character, position, color);
        blocksToSpawn.Enqueue(newBlockData);
    }
}
