using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "PlayerSettings")]
public class PlayerSettingsScriptableObject : ScriptableObject
{
    public float volume;

    public void SetVolume(float newVolume)
    {
        volume = newVolume;
        SetAudioListenerVolume(volume);
    }

    private static void SetAudioListenerVolume(float newVolume)
    {
        AudioListener.volume = newVolume;
    }
}
