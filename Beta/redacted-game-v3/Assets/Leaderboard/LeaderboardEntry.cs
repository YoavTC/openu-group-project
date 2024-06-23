using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardEntry : MonoBehaviour
{
    public int index;
    public string username;
    public int score;

    public void SetupEntry(int newIndex, string newUsername, int newScore)
    {
        index = newIndex;
        username = newUsername;
        score = newScore;
    }

    private void Start()
    {
        transform.GetChild(0).GetComponent<TMP_Text>().text = HelperFunctions.ConvertToOrdinal(index);
        transform.GetChild(1).GetComponent<TMP_Text>().text = username;
        transform.GetChild(2).GetComponent<TMP_Text>().text = score.ToString();
    }

}
