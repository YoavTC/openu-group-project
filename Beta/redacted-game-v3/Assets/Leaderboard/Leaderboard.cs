using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dan.Main;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    [Header("Leaderboard Loading Logic")] 
    [SerializeField] private bool loadLeaderboard;
    [SerializeField] private GameObject entryPrefab;
    [SerializeField] private Transform leaderboardEntriesContainer;
    [SerializeField] private Transform loadingAnimation;
    [SerializeField] private float loadingAnimationSpeed;

    private readonly string publicLeaderboardKey = "5a97bceda0f4f224409e575843a1ad908f7d1577edfe646c681589a68f8eb089";

    private void Start()
    {
        if (loadLeaderboard) PopulateLeaderboard();
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

    [Header("User Submission Logic")] 
    [SerializeField] private int maxUsernameLength;
    [SerializeField] private int minUsernameLength;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private IntFieldScriptableObject timeIntField;
    [SerializeField] private TMP_Text errorMessageDisplay;
    
    public void SubmitTime()
    {
        string username = inputField.text.Substring(0, Math.Min(maxUsernameLength, inputField.text.Length));
        InvalidUsernameErrorMessage errorCheck = isValidName(username);
        if (errorCheck.isValid)
        {
            LeaderboardCreator.UploadNewEntry(publicLeaderboardKey,
                username, timeIntField.intField, delegate(bool successful)
                {
                    if (successful)
                    {
                        Debug.Log("Entry uploaded successfully!");
                        DisplayMessage("Entry Submitted!", Color.green);
                    }
                    else
                    {
                        DisplayMessage("Server Error!\nFailed to upload entry!" + errorCheck.message, Color.red);
                        Debug.Log("Failed to upload entry!");
                    }
                });
            
            DisplayMessage("Submitting entry...", Color.yellow);
        } else {
            //InGameLogger.Log("Invalid name: " + errorCheck.message, Color.red);
            DisplayMessage(errorCheck.message, Color.red);
        }
    }

    private InvalidUsernameErrorMessage isValidName(string username)
    {
        if (username == string.Empty) return new InvalidUsernameErrorMessage("Name cannot contain spaces", false);
        if (IsBadWord(username)) return new InvalidUsernameErrorMessage("Name cannot bad words", false);

        TextAsset allowedCharacters = Resources.Load<TextAsset>("allowed_characters");
        if (allowedCharacters != null)
        {
            List<string> entries = allowedCharacters.text.Split('\n').ToList();
            entries = entries.Where(entry => !entry.Trim().StartsWith("/")).ToList();
            List<char> chars = entries.Select(entry => entry.Trim().FirstOrDefault()).ToList();

            foreach (char letter in username)
            {
                if (!chars.Contains(letter)) return new InvalidUsernameErrorMessage("Name cannot contain invalid characters", false);
            }
        }

        if (username.Length <= minUsernameLength) return new InvalidUsernameErrorMessage("Name is too short", false);;
        if (username.Length >= maxUsernameLength) return new InvalidUsernameErrorMessage("Name is too long", false);
        return new InvalidUsernameErrorMessage(true);
    }

    private bool IsBadWord(string word)
    {
        TextAsset badWordsAsset = Resources.Load<TextAsset>("bad_words");
        if (badWordsAsset != null)
        {
            string[] lines = badWordsAsset.text.Split('\n');

            foreach (string line in lines)
            {
                if (!line.StartsWith("//") && word.ToLower().Contains(line)) return true;
            }
        }
        return false;
    }

    private void DisplayMessage(string message, Color color)
    {
        errorMessageDisplay.DOKill();
        color.a = 0f;
        errorMessageDisplay.text = message;
        errorMessageDisplay.color = color;

        errorMessageDisplay.DOFade(1, 0.2f).OnComplete(() =>
        {
            errorMessageDisplay.DOFade(0, 3f).SetDelay(4);
        });
    }
}

public class InvalidUsernameErrorMessage
{
    public string message;
    public bool isValid;
    
    public InvalidUsernameErrorMessage(string message, bool isValid)
    {
        this.message = message;
        this.isValid = isValid;
    }
    
    public InvalidUsernameErrorMessage(bool isValid)
    {
        this.isValid = isValid;
    }
}
