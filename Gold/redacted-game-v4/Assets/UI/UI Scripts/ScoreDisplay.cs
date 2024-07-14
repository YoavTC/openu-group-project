using System;
using System.Collections;
using LootLocker.Requests;
using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private IntFieldScriptableObject timerField;
    private string leaderboardKey;

    public void DisplayInfo() => StartCoroutine(DisplayInfoRoutine());
    
    private IEnumerator DisplayInfoRoutine()
    {
        //Display score
        TMP_Text scoreDisplay = GetComponent<TMP_Text>();
        scoreDisplay.text = NumToDisplayableTime(timerField.intField);

        //Display rank
        TMP_Text rankDisplay = transform.GetChild(0).GetComponent<TMP_Text>();
        string rank = "???";
        rankDisplay.text = rank;
        
        //Get rank
        bool done = false;
        string memberID = PlayerPrefs.GetString("PlayerID");
        LootLockerSDKManager.GetMemberRank(leaderboardKey, memberID, response =>
        {
            if (response.success)
            {
                rank = HelperFunctions.ConvertToOrdinal(response.rank + 1);
            }
            done = true;
        });

        yield return new WaitUntil(() => done);
        
        rankDisplay.text = rank;
    }
    
    private string NumToDisplayableTime(float t)
    {
        TimeSpan timeSpan = TimeSpan.FromMilliseconds(t);
        string displayTime = string.Format("{0:00}:{1:00}:{2:000}", timeSpan.Minutes, timeSpan.Seconds,
            timeSpan.Milliseconds);
        return displayTime;
    }
}
