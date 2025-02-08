using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundSource : MonoBehaviour
{
    public AudioSource audioSource;

    private Coroutine returnToPoolCoroutine;
    private SoundFadeHandler fadeScript;

    private bool applicationIsQuitting = false;
    private bool isReturnedToPool = false;

    private AudioListener audioListener;

    private void OnApplicationQuit()
    {
        applicationIsQuitting = true;
    }

    private void Awake()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (!audioSource.isPlaying && !audioSource.loop)
        {
            ReturnToPoolInstant();
        }

        if (audioListener != null)
        {
            if (audioSource.maxDistance < Vector3.Distance(transform.position, audioListener.gameObject.transform.position))
            {
                ReturnToPoolInstant();
            }
        }
        else
        {
            audioListener = FindAnyObjectByType<AudioListener>();
        }

        if (!transform.parent.gameObject.activeSelf)
        {
            transform.SetParent(null);
        }
    }

    private void OnEnable()
    {
        isReturnedToPool = false;
        if (gameObject.transform.parent.TryGetComponent(out SoundCaller soundCaller))
        {
            SetUpAndPlay(soundCaller.soundData);
        }
        else
        {
            Debug.LogError("Sound caller wasn't found on this object: " + gameObject.transform.parent.name, this);
        }
    }

    private void OnDisable()
    {
        if (applicationIsQuitting)
        {
            return;
        }

        ReturnToPoolInstant();
    }

    private void PlaySound()
    {
        audioSource.Play();
        returnToPoolCoroutine = StartCoroutine(ReturnToPool());
    }

    private void PlaySoundFade(float fadeTime, float maxVolume)
    {
        audioSource.Play();
        returnToPoolCoroutine = StartCoroutine(ReturnToPool());
        fadeScript = gameObject.AddComponent<SoundFadeHandler>(); // decorator pattern
        fadeScript.SetAttributes(fadeTime, maxVolume);
    }

    private bool SetUpSoundData(SoundData soundData)
    {
        if (soundData == null)
        {
            Debug.LogError("Sound Data is null", this);
            return false; // unsuccessful
        }

        audioSource.clip = soundData.clip;
        audioSource.outputAudioMixerGroup = soundData.audioMixerGroup;
        audioSource.volume = soundData.volume;
        audioSource.pitch = soundData.pitch;
        audioSource.loop = soundData.loop;
        audioSource.spatialBlend = soundData.spatialBlend;
        audioSource.minDistance = soundData.minDistance;
        audioSource.maxDistance = soundData.maxDistance;
        audioSource.rolloffMode = soundData.rolloffMode;
        audioSource.priority = soundData.priority;

        if (soundData.randomPitch)
        {
            audioSource.pitch = Random.Range(soundData.minPitch, soundData.maxPitch);
        }

        return true; // successful
    }

    private void SetUpAndPlay(SoundData soundData)
    {
        if (SetUpSoundData(soundData))
        {
            if (soundData.fade)
            {
                PlaySoundFade(soundData.fadeTime, soundData.volume);
            }
            else
            {
                PlaySound();
            }
        }
    }

    private IEnumerator ReturnToPool()
    {
        if (!audioSource.loop)
        {
            yield return new WaitForSeconds(audioSource.clip.length / Time.timeScale);
            ReturnToPoolInstant();
        }
        else
        {
            yield break;
        }
    }

    private void ReturnToPoolInstant()
    {
        if (isReturnedToPool)
        {
            return;
        }
        else
        {
            isReturnedToPool = true;
        }

        if (returnToPoolCoroutine != null)
        {
            StopCoroutine(returnToPoolCoroutine);
        }
        if (fadeScript != null)
        {
            Destroy(fadeScript);
        }

        Debug.Log("Adding back to pool.", this);
        SFXPool.Instance.AddToPool(gameObject);
    }
}