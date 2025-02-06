using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum AudioType
{
    Master,
    Music,
    SFX
}

public class AudioSettings : MonoBehaviour
{
    [Header("Default Settings")]
    [SerializeField] private float defaultMasterVolume = 50;
    [SerializeField] private float defaultMusicVolume = 50;
    [SerializeField] private float defaultSFXVolume = 50;

    [Header("References")]
    [SerializeField] private AudioMixerGroup masterMixerGroup;
    [SerializeField] private AudioMixerGroup musicMixerGroup;
    [SerializeField] private AudioMixerGroup sfxMixerGroup;

    [Header("Current Volume Settings")]
    public float currentMasterVolume = 50;
    public float currentMusicVolume = 50;
    public float currentSFXVolume = 50;

    public void SetVolume(AudioType audioType, float value)
    {
        float decibelValue = ConvertToDecibels(value);

        switch (audioType)
        {
            case AudioType.Master:
                if (masterMixerGroup != null && masterMixerGroup.audioMixer != null)
                {
                    masterMixerGroup.audioMixer.SetFloat("MasterVolume", decibelValue);
                }
                PlayerPrefs.SetFloat("MasterVolume", value); // Save normalized value
                Debug.Log($"Master volume set to {value} ({decibelValue} dB)");
                currentMasterVolume = value;
                break;

            case AudioType.Music:
                if (musicMixerGroup != null && musicMixerGroup.audioMixer != null)
                {
                    musicMixerGroup.audioMixer.SetFloat("MusicVolume", decibelValue);
                }
                PlayerPrefs.SetFloat("MusicVolume", value); // Save normalized value
                Debug.Log($"Music volume set to {value} ({decibelValue} dB)");
                currentMusicVolume = value;
                break;

            case AudioType.SFX:
                if (sfxMixerGroup != null && sfxMixerGroup.audioMixer != null)
                {
                    sfxMixerGroup.audioMixer.SetFloat("SFXVolume", decibelValue);
                }
                PlayerPrefs.SetFloat("SFXVolume", value); // Save normalized value
                Debug.Log($"SFX volume set to {value} ({decibelValue} dB)");
                currentSFXVolume = value;
                break;
        }
    }

    private float ConvertToDecibels(float value)
    {
        value = Mathf.Clamp(value, 0, 100); // clamp value between 0 and 100

        if (value <= 0)
        {
            return -80f; // lowest decibel value
        }
        return Mathf.Log10(value / 100) * 20f; // convert normalized value to decibels
    }

    public void SetSettingsFromPrefs()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            SetVolume(AudioType.Master, PlayerPrefs.GetFloat("MasterVolume"));
        }
        else
        {
            SetVolume(AudioType.Master, defaultMasterVolume);
            PlayerPrefs.SetFloat("MasterVolume", defaultMasterVolume);
        }

        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            SetVolume(AudioType.Music, PlayerPrefs.GetFloat("MusicVolume"));
        }
        else
        {
            SetVolume(AudioType.Music, defaultMusicVolume);
            PlayerPrefs.SetFloat("MusicVolume", defaultMusicVolume);
        }

        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            SetVolume(AudioType.SFX, PlayerPrefs.GetFloat("SFXVolume"));
        }
        else
        {
            SetVolume(AudioType.SFX, defaultSFXVolume);
            PlayerPrefs.SetFloat("SFXVolume", defaultSFXVolume);
        }
    }
}