using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPool : MonoBehaviour
{
    public static SFXPool Instance { get; private set; }

    private Queue<GameObject> sfxPool = new Queue<GameObject>();

    [Header("SFX Pool Settings")]
    [SerializeField] private GameObject sfxPrefab;
    public int currentPoolSize { get; private set; }
    [SerializeField] private int poolSize = 10;
    [SerializeField] private bool expandable = true;
    [SerializeField] private int trueMaxSize = 30;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        currentPoolSize = 0;
        FillPool(poolSize);
    }

    private void FillPool(int amount)
    {
        if (expandable)
        {
            amount = Mathf.Clamp(amount, 0, trueMaxSize);
        }
        else
        {
            amount = Mathf.Clamp(amount, 0, poolSize);
        }

        for (int i = 0; i < amount; i++)
        {
            GameObject newSFX = Instantiate(sfxPrefab, transform);
            AddToPool(newSFX);
        }
    }

    public void AddToPool(GameObject returningObject)
    {
        if (currentPoolSize < trueMaxSize && expandable)
        {
            DeactivateObject(returningObject);
            sfxPool.Enqueue(returningObject);
            currentPoolSize++;
            Debug.Log($"Added to pool. // Pool Size: {currentPoolSize}/{trueMaxSize}");
        }
        else if (currentPoolSize < poolSize && !expandable)
        {
            DeactivateObject(returningObject);
            sfxPool.Enqueue(returningObject);
            currentPoolSize++;
            Debug.Log($"Added to pool. // Pool Size: {currentPoolSize}/{poolSize}");
        }
        else
        {
            Destroy(returningObject);
            Debug.LogWarning($"Pool is full ({currentPoolSize}/{trueMaxSize}), object destroyed");
        }
    }

    public GameObject TakeFromPool()
    {
        GameObject dequeuedObject = sfxPool.Dequeue();
        currentPoolSize--;
        Debug.Log($"Removed from pool. // Pool Size: {currentPoolSize}/{trueMaxSize}");
        return dequeuedObject;
    }

    private void DeactivateObject(GameObject returningObject)
    {
        returningObject.transform.SetParent(transform);
        returningObject.SetActive(false);
    }

    public void ExpandPool()
    {
        if (expandable && currentPoolSize < trueMaxSize)
        {
            FillPool(1);
        }
    }
}