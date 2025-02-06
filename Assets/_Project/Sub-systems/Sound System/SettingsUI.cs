using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [Header("References")]
    private AudioSettings audioSettings;
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private GameObject settingsPanel;

    private void Awake()
    {
        audioSettings = FindAnyObjectByType<AudioSettings>();

        if (audioSettings == null)
        {
            Debug.LogError("AudioSettings script not found in the scene.", this);
        }
    }

    private void Start()
    {
        audioSettings.SetSettingsFromPrefs();
        UpdateSliders();
    }

    public void ToggleSettingsPanel()
    {
        if (settingsPanel.activeSelf)
        {
            settingsPanel.SetActive(false);
        }
        else
        {
            settingsPanel.SetActive(true);
        }
    }

    private void UpdateSliders()
    {
        masterVolumeSlider.value = audioSettings.currentMasterVolume;
        musicVolumeSlider.value = audioSettings.currentMusicVolume;
        sfxVolumeSlider.value = audioSettings.currentSFXVolume;
    }

    public void SetMasterVolume()
    {
        audioSettings.SetVolume(AudioType.Master, masterVolumeSlider.value);
    }

    public void SetMusicVolume()
    {
        audioSettings.SetVolume(AudioType.Music, musicVolumeSlider.value);
    }

    public void SetSFXVolume()
    {
        audioSettings.SetVolume(AudioType.SFX, sfxVolumeSlider.value);
    }
}
