using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private IntFieldScriptableObject timerField;
    private TMP_Text display;
    
    void Start()
    {
        display = GetComponent<TMP_Text>();
        TimeSpan runTimeSpan = TimeSpan.FromMilliseconds(timerField.intField);
        display.text = runTimeSpan.ToString();
    }
}
