using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCaller : MonoBehaviour
{
    public SoundType soundType;
    public SoundData soundData;

    private void Start()
    {
        if (soundType == SoundType.OnStart)
        {
            PlaySound();
        }
    }

    public void CallSound()
    {
        if (soundType == SoundType.OnCall)
        {
            PlaySound();
        }
    }

    private void PlaySound()
    {
        SoundRequest soundRequest = new SoundRequest
        {
            soundData = soundData,
            position = transform.position
        };
        SFXManager.Instance.AddSoundToQueue(gameObject, soundData, transform.position);
    }
}
