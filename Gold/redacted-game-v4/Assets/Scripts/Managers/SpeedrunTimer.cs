using System;
using TMPro;
using UnityEngine;

public class SpeedrunTimer : MonoBehaviour
{
    [SerializeField] private IntFieldScriptableObject intTime;
    private TMP_Text timerDisplay;
    private bool timerRunning = true;
    private float elapsedTime;
    
    void Start()
    {
        timerDisplay = GetComponent<TMP_Text>();
    }

    
    void Update()
    {
        if (timerRunning)
        {
            elapsedTime += Time.deltaTime;
            TimeSpan timeSpan = TimeSpan.FromSeconds(elapsedTime);
            timerDisplay.text = string.Format("{0:00}:{1:00}:{2:000}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
        }
    }

    public void OnGamePause()
    {
        timerRunning = false;
    }

    public void OnGameUnpaused()
    {
        timerRunning = true;
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
    }

    public void NewTimer()
    {
        intTime.intField = (int) TimeSpan.FromSeconds(elapsedTime).TotalMilliseconds;
        timerRunning = false;
    }
}
