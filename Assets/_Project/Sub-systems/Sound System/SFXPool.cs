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
            AddToPool(Instantiate(sfxPrefab, transform));
        }
    }

    public void AddToPool(GameObject returningObject)
    {
        if (expandable)
        {
            if (sfxPool.Count < trueMaxSize)
            {
                SanitiseObject(returningObject);
                sfxPool.Enqueue(returningObject);
                currentPoolSize++;
            }
            else
            {
                Destroy(returningObject);
                Debug.LogWarning("Pool is full, object destroyed");
            }
        }
        else
        {
            if (sfxPool.Count < poolSize)
            {
                SanitiseObject(returningObject);
                sfxPool.Enqueue(returningObject);
                currentPoolSize++;
            }
            else
            {
                Destroy(returningObject);
                Debug.LogWarning("Pool is full, object destroyed");
            }
        }
    }

    public GameObject TakeFromPool()
    {
        GameObject dequeuedObject = sfxPool.Dequeue();
        currentPoolSize--;
        return dequeuedObject;
    }

    private void SanitiseObject(GameObject returningObject)
    {
        returningObject.SetActive(false);
        returningObject.transform.SetParent(transform);
    }
}