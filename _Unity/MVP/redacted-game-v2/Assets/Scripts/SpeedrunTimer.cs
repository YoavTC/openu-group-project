using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedrunTimer : MonoBehaviour
{
    private TMP_Text timerDisplay;
    private DateTime timeStarted;
    
    void Start()
    {
        timerDisplay = GetComponent<TMP_Text>();
        timeStarted = DateTime.Now;
    }

    
    void Update()
    {
        //timerDisplay.text = Time.timeSinceLevelLoad.ToString("0.000");
        TimeSpan newDateTime = TimeSpan.FromSeconds(Time.timeSinceLevelLoad);
        //TimeSpan newDateTime = DateTime.Now - timeStarted;
        timerDisplay.text = string.Format("{0:00}:{1:00}:{2:000}", newDateTime.Minutes, newDateTime.Seconds, newDateTime.Milliseconds);
    }
}
