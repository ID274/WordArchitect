using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundSource : MonoBehaviour
{
    public AudioSource audioSource;

    private void Awake()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    private void OnEnable()
    {
        if (gameObject.transform.parent.TryGetComponent(out SoundCaller soundCaller))
        {
            SetUpAndPlay(soundCaller.soundData);
        }
        else
        {
            Debug.LogError("Sound caller wasn't found on this object: " + gameObject.transform.parent.name, this);
        }
    }
    private void PlaySound()
    {
        audioSource.Play();
        StartCoroutine(ReturnToPool());
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
            PlaySound();
        }
    }

    private IEnumerator ReturnToPool()
    {
        if (!audioSource.loop)
        {
            yield return new WaitForSeconds(audioSource.clip.length * Time.timeScale);
            SFXPool.Instance.AddToPool(gameObject);
        }
        else
        {
            // unimplemented functionality, so for now it just never returns to pool
            Debug.LogWarning("Looping sound is not yet implemented properly");
        }
    }
}
