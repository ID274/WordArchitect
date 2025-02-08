using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "NewSoundData", menuName = "Sound System/Sound Data", order = 1)]
public class SoundData : ScriptableObject
{
    [Range(256, 0)]public int priority = 128;

    [Header("General Settings")]
    public AudioClip clip;
    public AudioMixerGroup audioMixerGroup;
    public float volume = 1;
    public float pitch = 1;

    [Header("Loop Settings")]
    public bool loop;

    [Header("Random Pitch")]
    public bool randomPitch;
    public float minPitch = 0.9f;
    public float maxPitch = 1.1f;

    [Header("Spacial Settings")]
    [Range(0, 1)] public float spatialBlend = 0;
    public float minDistance = 1;
    public float maxDistance = 500;
    public AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic;

    [Header("Fade Settings")]
    public bool fade;
    public float fadeTime = 0;
}
