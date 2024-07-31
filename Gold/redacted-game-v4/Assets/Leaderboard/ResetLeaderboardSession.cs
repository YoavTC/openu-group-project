using System.Collections;
using System.Collections.Generic;
using LootLocker.Requests;
using UnityEngine;

public class ResetLeaderboardSession : MonoBehaviour
{
    void Start()
    {
        PlayerPrefs.DeleteKey("LootLockerGuestPlayerID");
    }
}
