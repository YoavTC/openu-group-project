using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundSystem : Singleton<SoundSystem>
{
    private List<GameObject> soundPlayers = new List<GameObject>();
    
    public void PlaySound(AudioClip clip, bool loop = false)
    {
        GameObject soundPlayer = new GameObject("sfx: " + clip.name);
        AudioSource audioSource = soundPlayer.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.loop = loop;
        audioSource.Play();
        if (!loop) StartCoroutine(YieldDestroy(clip.length, soundPlayer));
    }

    public void StopSound(AudioClip clip, float delay)
    {
        AudioSource audioSource = soundPlayers.First(i => i.GetComponent<AudioSource>().clip == clip).GetComponent<AudioSource>();
        StartCoroutine(YieldStop(delay, audioSource));
    }

    private IEnumerator YieldDestroy(float length, GameObject audioPlayer)
    {
        yield return HelperFunctions.GetWaitRealTime(length);
        Destroy(audioPlayer);
    }

    private IEnumerator YieldStop(float delay, AudioSource audioSource)
    {
        yield return HelperFunctions.GetWaitRealTime(delay);
        audioSource.Stop();
    }
}
