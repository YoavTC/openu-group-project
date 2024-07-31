using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    [SerializeField] private AudioClip jump;
    [SerializeField] private AudioClip slide;
    [SerializeField] private AudioClip[] speedUpgrades;
    [SerializeField] private AudioClip jump3;
    [SerializeField] private AudioClip jump4;
    [SerializeField] private AudioClip jump5;
    [SerializeField] private AudioClip jump6;
    
    public void OnJump() => SoundSystem.Instance.PlaySound(jump);
    
    public void OnSlideActivate() => SoundSystem.Instance.PlaySound(slide, true, 0.2f);
    public void OnSlideDeactivate() => SoundSystem.Instance.StopSound(slide, 0f);

    public void OnSpeedBoostUpgrade(int level)
    {
        SoundSystem.Instance.PlaySound(speedUpgrades[level]);
    }
    public void OnSpeedBoostDowngrade()
    {
        SoundSystem.Instance.PlaySound(speedUpgrades[0]);
    }
}
