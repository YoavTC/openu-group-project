using Udar.SceneManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private SceneField playScene, leaderboardScene, settingsScene, aboutScene, mainMenuScene;
    
    public void PressMainMenuButton() => SceneManager.LoadScene(mainMenuScene.BuildIndex);
    public void PressPlayButton() => SceneManager.LoadScene(playScene.BuildIndex);
    public void PressLeaderboardButton() => SceneManager.LoadScene(leaderboardScene.BuildIndex);
    public void PressSettingsButton() => SceneManager.LoadScene(settingsScene.BuildIndex);
    public void PressAboutButton() => SceneManager.LoadScene(aboutScene.BuildIndex);
    public void PressQuitButton() {}
}
