using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "PlayerSettings")]
public class PlayerSettingsScriptableObject : ScriptableObject
{
    public bool isSpeedrun;
    public float volume;

    public void SetSpeedrunMode(bool state)
    {
        isSpeedrun = state;
    }

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
