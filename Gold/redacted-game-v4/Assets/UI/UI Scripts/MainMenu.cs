using System;
using Udar.SceneManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using DG.Tweening;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Image fade;
    [SerializeField] private PlayerSettingsScriptableObject playerSettingsScriptableObject;

    private void Start()
    {
        AudioListener.volume = playerSettingsScriptableObject.volume;
    }

    private void GoToBlack()
    {
        fade.DOFade(0f, 0f);
        fade.DOFade(1f, 2f).OnComplete(() =>
        {
            SceneManager.LoadScene(1);
        });
    }
    
    public void PressMainMenuButton() => SceneManager.LoadScene(0);
    public void PressComicButton()
    {
        fade.enabled = true;
        GoToBlack();
    }

    public void PressPlayButton() => SceneManager.LoadScene(2);
    public void PressLeaderboardButton() => SceneManager.LoadScene(3);
    public void PressAboutButton() => SceneManager.LoadScene(4);
    public void PressSettingsButton() => SceneManager.LoadScene(5);

    public void PressQuitButton()
    {
#if UNITY_EDITOR
        
#else
        Application.Quit();
#endif
    }

}
