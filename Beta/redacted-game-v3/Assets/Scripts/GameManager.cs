using System;
using System.Collections;
using System.Collections.Generic;
using Udar.SceneManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SceneField gameScene;
    [SerializeField] private PlayerMovement playerController;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerMovement>();
    }
    
    public void OnPlayerRespawnEvent()
    {
        //Alert checkpoint system
    }
}
