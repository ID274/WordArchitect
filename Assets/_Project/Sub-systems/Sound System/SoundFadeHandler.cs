using System.Collections;
using UnityEngine;

public class SoundFadeHandler : MonoBehaviour
{
    private AudioSource audioSource;
    public float fadeTime { get; private set; }
    public float maxVolume { get; private set; }
    private float clipLength;

    private Coroutine fadeCoroutine;

    public void SetAttributes(float fadeTime, float maxVolume)
    {
        audioSource = GetComponent<AudioSource>();
        this.fadeTime = fadeTime;
        this.maxVolume = maxVolume;
        clipLength = audioSource.clip.length;

        if (audioSource == null)
        {
            Debug.LogError("Audio Source is null", this);
            return;
        }
        else
        {
            Fade(fadeTime);
        }
    }

    public void Fade(float fadeOutTime)
    {
        fadeCoroutine = StartCoroutine(FadeCoroutine(fadeOutTime));
    }

    private IEnumerator FadeCoroutine(float fadeOutTime)
    {
        if (fadeOutTime > (clipLength / 2))
        {
            Debug.LogWarning("Fade out time is greater than clip length, ignoring fade", this);
            yield break;
        }

        // fade in
        audioSource.volume = 0f;

        while (audioSource.volume < maxVolume)
        {
            audioSource.volume += maxVolume * Time.deltaTime / fadeTime;
            yield return null;
        }
        audioSource.volume = maxVolume;

        // wait for the duration of the clip minus fade out time to get fade start time
        yield return new WaitForSeconds(clipLength - fadeOutTime);

        // fade out
        while (audioSource.volume > 0)
        {
            audioSource.volume -= maxVolume * Time.deltaTime / fadeOutTime;
            yield return null;
        }
        audioSource.volume = 0f;

        if (audioSource.loop)
        {
            fadeCoroutine = StartCoroutine(FadeCoroutine(fadeTime));
        }
    }

    private void OnDestroy()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
    }
}
