using System;
using TMPro;
using UnityEngine;

public class SpeedrunTimer : MonoBehaviour
{
    [SerializeField] private PlayerSettingsScriptableObject playerSettings;
    
    private TMP_Text timerDisplay;
    
    private bool timerRunning = true;
    private float elapsedTime;
    
    void Start()
    {
        timerDisplay = GetComponent<TMP_Text>();
        gameObject.SetActive(playerSettings.showTimer);
    }

    
    void Update()
    {
        if (timerRunning)
        {
            elapsedTime += Time.deltaTime;
            TimeSpan timeSpan = TimeSpan.FromSeconds(elapsedTime);
            timerDisplay.text = string.Format("{0:00}:{1:00}:{2:000}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
        }
        
        // TimeSpan newDateTime = TimeSpan.FromSeconds(Time.timeSinceLevelLoad);
        // timerDisplay.text = string.Format("{0:00}:{1:00}:{2:000}", newDateTime.Minutes, newDateTime.Seconds, newDateTime.Milliseconds);
    }

    public void OnGamePause()
    {
        timerRunning = false;
        //gameObject.SetActive(playerSettings.showTimer);
        timerDisplay.enabled = playerSettings.showTimer;
    }

    public void OnGameUnpaused()
    {
        timerRunning = true;
        //gameObject.SetActive(playerSettings.showTimer);
        timerDisplay.enabled = playerSettings.showTimer;
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
    }
}
