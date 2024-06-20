using System.Collections;
using System.Collections.Generic;
using Udar.SceneManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private SceneField playScene;
    [SerializeField] private SceneField mainMenuScene;

    public void PlayGame()
    {
        SceneManager.LoadScene(playScene.BuildIndex);
    }
    
    public void GoMainMenu()
    {
        FindObjectOfType<PauseHandler>().gameUnpaused?.Invoke();
        SceneManager.UnloadSceneAsync(playScene.BuildIndex);
        SceneManager.LoadScene(mainMenuScene.BuildIndex);
    }
}
