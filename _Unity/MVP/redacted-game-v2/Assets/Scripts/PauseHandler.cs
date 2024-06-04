using System;
using System.Collections;
using System.Collections.Generic;
using Udar.SceneManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseHandler : MonoBehaviour
{
    [Header("Scenes")] 
    [SerializeField] private SceneField pauseMenuScene;
    [SerializeField] private SceneField gameScene; 
    
    [SerializeField] private KeyCode pauseShortcut;
    public bool isPaused { get; private set; }

    private void Start()
    {
        DontDestroyOnLoad(transform);
    }

    void Update()
    {
        if (Input.GetKeyDown(pauseShortcut))
        {
            if (!isPaused)
            {
                //Pauses the game
                Time.timeScale = 0f;
                isPaused = true;
            
                //Transition to menu scene
                SceneManager.LoadSceneAsync(pauseMenuScene.BuildIndex, LoadSceneMode.Additive);
            } else {
                //Unpauses the game
                Time.timeScale = 1f;
                isPaused = false;
            
                //Transition to menu scene
                SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(pauseMenuScene.BuildIndex));
                SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            }
        }
    }
}
