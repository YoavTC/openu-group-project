using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardEntry : MonoBehaviour
{
    public int index;
    public string username;
    public int time;

    public void SetupEntry(int newIndex, string newUsername, int newTime)
    {
        index = newIndex;
        username = newUsername;
        time = newTime;
    }

    private void Start()
    {
        transform.GetChild(0).GetComponent<TMP_Text>().text = HelperFunctions.ConvertToOrdinal(index);
        transform.GetChild(1).GetComponent<TMP_Text>().text = username;
        transform.GetChild(2).GetComponent<TMP_Text>().text = FloatToDisplayableTime(time);
    }

    private string FloatToDisplayableTime(float t)
    {
        TimeSpan timeSpan = TimeSpan.FromMilliseconds(t);
        string displayTime = string.Format("{0:00}:{1:00}:{2:000}", timeSpan.Minutes, timeSpan.Seconds,
            timeSpan.Milliseconds);
        return displayTime;
    }
}
