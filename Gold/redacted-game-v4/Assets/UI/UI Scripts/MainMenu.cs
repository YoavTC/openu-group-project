using Udar.SceneManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private SceneField playScene, leaderboardScene, settingsScene, aboutScene, mainMenuScene;
    
    public void PressMainMenuButton() => SceneManager.LoadScene(0);
    public void PressPlayButton() => SceneManager.LoadScene(2);
    public void PressLeaderboardButton() => SceneManager.LoadScene(3);
    public void PressSettingsButton() => SceneManager.LoadScene(settingsScene.BuildIndex);
    public void PressAboutButton() => SceneManager.LoadScene(aboutScene.BuildIndex);
    public void PressQuitButton() {}

    public void AAHHHHH() => SceneManager.LoadScene(1);
}
