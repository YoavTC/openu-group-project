using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsInitializer : MonoBehaviour
{
    [SerializeField] private TMP_Text display;
    [SerializeField] private Scrollbar scrollbar;
    [SerializeField] private PlayerSettingsScriptableObject playerSettingsScriptableObject;
    void Start()
    {
        float newVolume = playerSettingsScriptableObject.volume;
        scrollbar.value = newVolume;
        OnVolumeChanged(newVolume);
    }

    public void OnVolumeChanged(float newVolume)
    {
        display.text = "Volume:\n" + newVolume.ToString("P0");
    }
}
