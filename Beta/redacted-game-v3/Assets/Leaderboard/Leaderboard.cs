using System;
using System.Collections;
using System.Collections.Generic;
using Dan.Main;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private GameObject entryPrefab;
    [SerializeField] private Transform leaderboardEntriesContainer;
    [SerializeField] private Transform loadingAnimation;
    [SerializeField] private float loadingAnimationSpeed;

    private string publicLeaderboardKey = "34dbd0e601d7b357b80d669d7c73db361f01850fa5f2f67574ca355cd29fa258";

    private void Start()
    {
        PopulateLeaderboard();
    }

    private void PopulateLeaderboard()
    {
        HelperFunctions.DestroyChildren(leaderboardEntriesContainer);
        LeaderboardCreator.GetLeaderboard(publicLeaderboardKey, (leaderboard) =>
        {
            Debug.Log("Got leaderboard: " + leaderboard);
            for (int i = 0; i < leaderboard.Length; i++)
            {
                Debug.Log("Added entry: " + i + ", " + leaderboard[i].Username + leaderboard[i].Score);
                LeaderboardEntry leaderboardEntry = Instantiate(entryPrefab, leaderboardEntriesContainer).GetComponent<LeaderboardEntry>();
                leaderboardEntry.SetupEntry(leaderboard[i].Rank, leaderboard[i].Username, leaderboard[i].Score);
            }
            
            StopLoadingAnimation();
        });

        loadingAnimation.parent.gameObject.SetActive(true);
        loadingAnimation.DORotate(new Vector3(0, 0, 360), loadingAnimationSpeed, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
    }

    private void StopLoadingAnimation()
    {
        loadingAnimation.DOKill();
        loadingAnimation.parent.gameObject.SetActive(false);
    }
}
