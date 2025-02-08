using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }

    [Header("Sound Requests")]
    private List<(int, SoundRequest)> soundRequestList = new List<(int, SoundRequest)>();
    [SerializeField] private int maxSoundsQueued = 20;

    [Header("Misc Settings")]
    [SerializeField] private bool destroyOnLoad = true;
    [SerializeField] private float timeBetweenSorts = 2f;

    private float timeSinceLastSort = 0f;

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

        if (!destroyOnLoad)
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void FixedUpdate()
    {
        timeSinceLastSort += Time.deltaTime;

        if (timeSinceLastSort >= timeBetweenSorts)
        {
            timeSinceLastSort = 0f;
            soundRequestList.Sort((a, b) => a.Item1.CompareTo(b.Item1));
        }

        if (SFXPool.Instance.currentPoolSize > 0)
        {
            PlaySoundFromPool();
        }

        if (soundRequestList.Count > 3)
        {
            Debug.Log($"Sound Request List: {soundRequestList.Count}, expanding pool by 1.");
            SFXPool.Instance.ExpandPool();
        }
    }

    public void AddSoundToQueue(GameObject objectRequesting, SoundData soundData, Vector3 position)
    {
        SoundRequest soundRequest = new SoundRequest
        {
            soundData = soundData,
            position = position,
            objectRequesting = objectRequesting
        };
        
        if (soundRequestList.Count < maxSoundsQueued)
        {
            AddToListAndSort(objectRequesting, soundRequest);
        }
        else
        {
            Debug.LogWarning("Sound Request List is full. Removing lowest priority sound");
            if (soundData.priority < soundRequestList[soundRequestList.Count - 1].Item1)
            {
                RemoveLastSoundFromQueue();
                AddToListAndSort(objectRequesting, soundRequest);
            }
        }
    }

    public void RemoveLastSoundFromQueue()
    {
        soundRequestList.RemoveAt(soundRequestList.Count - 1);
    }

    private void PlaySoundFromPool()
    {
        if (soundRequestList.Count > 0)
        {
            SoundRequest soundRequest = soundRequestList[0].Item2;
            soundRequestList.RemoveAt(0);
            GameObject soundObject = SFXPool.Instance.TakeFromPool();
            soundObject.transform.position = soundRequest.position;
            soundObject.transform.parent = soundRequest.objectRequesting.transform;
            soundObject.SetActive(true);
        }
    }

    private void AddToListAndSort(GameObject objectRequesting, SoundRequest soundRequest)
    {
        int priority = soundRequest.soundData.priority;

        soundRequestList.Add((priority, soundRequest));
    }
}
