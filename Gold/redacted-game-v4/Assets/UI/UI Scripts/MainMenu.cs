using Udar.SceneManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Image Fade;
    public void PressMainMenuButton() => SceneManager.LoadScene(0);

    public void PressComicButton()
    {
        Fade.enabled = true;
        StartCoroutine(GoToBlack());
    }

    private IEnumerator GoToBlack()
    {
        var elapsed = 0f;
        const float duration = 2f;

        while (elapsed < duration)
        {
            Fade.color = new Color(Fade.color.r, Fade.color.g, Fade.color.g,
                Mathf.MoveTowards(Fade.color.a, 255, 2f * Time.deltaTime));
            elapsed += Time.deltaTime;
            
            yield return null;
        }
        SceneManager.LoadScene(1);
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
