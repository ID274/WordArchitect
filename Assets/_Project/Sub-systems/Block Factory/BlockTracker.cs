using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockTracker : MonoBehaviour
{
    public static BlockTracker Instance { get; private set; }

    public Vector3 nextBlockPosition;

    public int blockCount = 0;

    Stack<GameObject> orderedBlocks = new Stack<GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public GameObject[] ReturnXBlocks(int amount)
    {
        // Create a temporary stack to avoid modifying the original stack
        Stack<GameObject> tempStack = new Stack<GameObject>(orderedBlocks);

        if (tempStack.Count > amount)
        {
            amount = tempStack.Count;
        }

        GameObject[] tempObjects = new GameObject[amount];

        // Peek and pop {amount} blocks from the temporary stack
        for (int i = 0; i < amount; i++)
        {
            if (tempStack.Count > 0)
            {
                tempObjects[i] = tempStack.Pop();
            }
            else
            {
                tempObjects[i] = null;
            }
        }

        return tempObjects;
    }

    public void AddBlock(GameObject block)
    {
        orderedBlocks.Push(block);
        SetNextBlockPosition(block.transform.Find("NextBlockPosition").position);
        blockCount++;
    }

    public void RemoveBlock(GameObject block)
    {
        orderedBlocks.Pop();
        blockCount--;
    }

    public void ClearBlocks()
    {
        orderedBlocks.Clear();
        blockCount = 0;
    }

    public void SetNextBlockPosition(Vector3 position)
    {
        nextBlockPosition = position;
    }
}