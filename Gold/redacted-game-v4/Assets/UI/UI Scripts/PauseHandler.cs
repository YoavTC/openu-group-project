using System;
using System.Collections;
using DG.Tweening;
using NaughtyAttributes;
using Udar.SceneManager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseHandler : MonoBehaviour
{
    [Header("Scenes")] 
    [SerializeField] private SceneField mainMenuScene;
    [SerializeField] private SceneField settingsScene;
    [SerializeField] private SceneField leaderboardScene;

    [Header("Settings")]
    [SerializeField] private KeyCode pauseKeyCode;
    [SerializeField] private float pauseCooldown;
    private bool canPause;
    private bool canInteract;

    [Header("Animation")] 
    [SerializeField] private Transform pausePanel;
    [SerializeField] private Image pauseBackground;
    [SerializeField] private float animationTime;
    [SerializeField] private Ease animationEaseType;
    [SerializeField] private Transform offScreenPosition;
    private Vector3 defaultPosition;
    [SerializeField] [ReadOnly] private bool isPaused;

    [Header("Events")] 
    public UnityEvent onUnpauseGameEvent;
    public UnityEvent onPauseGameEvent;

    private void Start()
    {
        canInteract = false;
        canPause = true;
        defaultPosition = transform.position;
        transform.position = offScreenPosition.position;
        
        UnpauseGame();
        
        #if UNITY_EDITOR
        #else
        pauseKeyCode = KeyCode.Escape;
        #endif
    }

    private void Update()
    {
        if (Input.GetKey(pauseKeyCode) && canPause)
        {
            StartCoroutine(PauseCooldown());
            
            if (isPaused) UnpauseGame();
            else PauseGame();
        }
    }

    private IEnumerator PauseCooldown()
    {
        canPause = false;
        yield return HelperFunctions.GetWaitRealTime(pauseCooldown);
        canPause = true;
    }

    #region Buttons
    
    public void ChangeSelector(Transform buttonTransform)
    {
        if (!canInteract) return;
        EventSystem.current.SetSelectedGameObject(buttonTransform.gameObject);
        //Play sound effect?
    }
    
    public void PressResumeGameButton()
    {
        if (!canInteract) return;
        UnpauseGame();
    }

    public void PressMainMenuButton()
    {
        if (!canInteract) return;
        UnpauseGame(false);
        SceneManager.LoadScene(0);
    }
    
    #endregion

    private void PauseGame()
    {
        isPaused = true;
        canInteract = true;
        onPauseGameEvent?.Invoke();

        Time.timeScale = 0f;

        //Animate enable pause dialog
        pausePanel.DOMove(defaultPosition, animationTime).SetEase(animationEaseType).SetUpdate(true);
        pauseBackground.DOFade(0.8f, animationTime).SetUpdate(true);
    }

    private void UnpauseGame(bool animate = true)
    {
        isPaused = false;
        canInteract = false;
        onUnpauseGameEvent?.Invoke();

        Time.timeScale = 1f;

        //Animate disable pause dialog
        if (animate) pausePanel.DOMove(offScreenPosition.position, animationTime).SetEase(animationEaseType).SetUpdate(true);
        if (animate) pauseBackground.DOFade(0f, animationTime).SetUpdate(true);

        EventSystem.current.SetSelectedGameObject(null);
    }
}
