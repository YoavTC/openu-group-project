using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSystem : Singleton<SoundSystem>
{
    [SerializeField] private AudioSource[] source;
    
    // Create enum for audio clip names
    
    public void Play(int curSource, AudioClip playableSfx, bool loop)
    {
        source[curSource].clip = playableSfx;
        source[curSource].loop = loop;
        source[curSource].Play();
    }

    public void PlayOneShot(int curSource, AudioClip playableSfx)
    {
        source[curSource].PlayOneShot(playableSfx);
    }

    public void PlayWithDelay(int curSource, AudioClip playableSfx, float delay)
    {
        source[curSource].clip = playableSfx;
        source[curSource].PlayDelayed(delay);
    }

    public void Pause(int curSource)
    {
        source[curSource].Pause();
    }

    public void VolumeChanger(int curSource, float adjVol)
    {
        source[curSource].volume = adjVol;
    }

    public void MuteToggle(int curSource)
    {
        source[curSource].mute = !source[curSource].mute;
    }
}
