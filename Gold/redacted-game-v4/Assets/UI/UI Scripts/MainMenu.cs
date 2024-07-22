using Udar.SceneManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PressMainMenuButton() => SceneManager.LoadScene(0);
    public void PressComicButton() => SceneManager.LoadScene(1);
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
