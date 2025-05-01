using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(BlockFactory))]
public class BlockSpawner : MonoBehaviour, IBlockObserver
{
    [SerializeField] private BlockFactory blockFactory;

    Queue<(char, Vector3, Color)> blocksToSpawn = new Queue<(char, Vector3, Color)>();

    [SerializeField] private float spawnInterval = 0.5f;
    private bool onCooldown = false;

    public static Action onBlockSpawn;

    private IBlockObserverSubject blockObserverSubject;

    private bool startPassed = false;

    private void Awake()
    {
        blockFactory = GetComponent<BlockFactory>();
    }

    private void Start()
    {
        blockObserverSubject = GrabBlockObserverSubject();
        blockObserverSubject.AddObserver(this);
        startPassed = true;
    }

    private void OnEnable()
    {
        if (startPassed)
        {
            blockObserverSubject.AddObserver(this);
        }
    }

    private void OnDisable()
    {
        blockObserverSubject.RemoveObserver(this);
    }

    private void FixedUpdate()
    {
        if (blocksToSpawn.Count > 0 && !onCooldown)
        {
            (char, Vector3, Color) blockData = blocksToSpawn.Dequeue();
            SpawnBlock(blockData.Item1, blockData.Item3);
            onBlockSpawn?.Invoke();
        }
    }

    private IEnumerator SpawnBlockCoroutine()
    {
        onCooldown = true;
        yield return new WaitForSeconds(spawnInterval);
        onCooldown = false;
    }

    private void SpawnBlock(char character, Color color)
    {
        SpawnBlock(character, BlockTracker.Instance.nextBlockPosition, color);
    }

    private void SpawnBlock(char character, Vector3 position, Color color)
    {
        GameObject newBlock = blockFactory.CreateLetterBlock(character, position, color);
        StartCoroutine(SpawnBlockCoroutine());
    }

    public void AddBlockToSpawn(char character, Color color)
    {
        AddBlockToSpawn(character, BlockTracker.Instance.nextBlockPosition, color);
    }

    private void AddBlockToSpawn(char character, Vector3 position, Color color)
    {
        (char, Vector3, Color) newBlockData = (character, position, color);
        blocksToSpawn.Enqueue(newBlockData);
    }

    // observer implementation
    public IBlockObserverSubject GrabBlockObserverSubject()
    {
        IBlockObserverSubject baseObserverSubjects = FindObjectOfType<BaseObserverSubject<IBlockObserver>>() as IBlockObserverSubject;

        if (baseObserverSubjects == null)
        {
            Debug.LogError("No BaseObserverSubject found.", this);
            return null;
        }

        return baseObserverSubjects;
    }

    public void OnBlockSpawn(char character, Color color) // implemented from IBlockObserver
    {
        AddBlockToSpawn(character, color);
    }
}
