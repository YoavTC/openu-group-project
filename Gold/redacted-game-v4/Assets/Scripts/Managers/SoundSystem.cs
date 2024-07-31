using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundSystem : Singleton<SoundSystem>
{
    [SerializeField] private List<GameObject> soundPlayers = new List<GameObject>();
    
    public void PlaySound(AudioClip clip, bool loop = false, float delay = 0f)
    {
        GameObject soundPlayer = new GameObject("sfx: " + clip.name);
        AudioSource audioSource = soundPlayer.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.loop = loop;
        
        soundPlayers.Add(soundPlayer);
        if (delay == 0f) audioSource.Play();
        else audioSource.PlayDelayed(delay);
        if (!loop) StartCoroutine(YieldDestroy(clip.length, soundPlayer));
    }

    public void StopSound(AudioClip clip, float delay)
    {
        AudioSource audioSource = soundPlayers.First(i => i.GetComponent<AudioSource>().clip == clip).GetComponent<AudioSource>();
        StartCoroutine(YieldStop(delay, audioSource));
    }

    private IEnumerator YieldDestroy(float length, GameObject audioPlayer)
    {
        soundPlayers.Remove(audioPlayer);
        yield return HelperFunctions.GetWaitRealTime(length);
        Destroy(audioPlayer);
    }

    private IEnumerator YieldStop(float delay, AudioSource audioSource)
    {
        soundPlayers.Remove(audioSource.gameObject);
        yield return HelperFunctions.GetWaitRealTime(delay);
        Destroy(audioSource.gameObject);
    }
}
