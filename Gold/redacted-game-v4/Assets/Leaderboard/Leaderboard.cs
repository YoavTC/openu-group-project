using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using LootLocker.Requests;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    
    [Header("LootLocker Leaderboard")] 
    [SerializeField] private int leaderboardID = 23411;
    [SerializeField] [ReadOnly] private bool isAuthenticated;
    private Leaderboard leaderboard;
    private readonly int authenticationTimeout = 70;
    [SerializeField] [ReadOnly] private bool isBetterScore;
    [SerializeField] [ReadOnly] private bool isBetterScoreCheck;
    
    [Header("Leaderboard Loading Logic")] 
    [SerializeField] private bool loadLeaderboard;
    [SerializeField] private GameObject entryPrefab;
    [SerializeField] private Transform leaderboardEntriesContainer;
    [SerializeField] private Transform loadingAnimation;
    [SerializeField] private float loadingAnimationSpeed;
    
    [Header("User Submission Logic")] 
    [SerializeField] private int maxUsernameLength;
    [SerializeField] private int minUsernameLength;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private IntFieldScriptableObject timeIntField;
    [SerializeField] private TMP_Text errorMessageDisplay;
    [SerializeField] private Image errorMessageBackground;

    [SerializeField] private float submitCooldown;
    [SerializeField] private bool canSubmit;

    private IEnumerator Start()
    {
        Time.timeScale = 1f;
        
        isBetterScore = false;
        isBetterScoreCheck = true;
        
        canSubmit = true;
        isAuthenticated = false;
        int authenticationTimer = 0;
        
        StartLoadingAnimation();

        StartCoroutine(LoginRoutine());
        while (!isAuthenticated || authenticationTimer >= authenticationTimeout)
        {
            InGameLogger.Log("Waiting for LootLocker Authentication Servers...", Color.yellow);
            authenticationTimer++;
            yield return HelperFunctions.GetWait(0.5f);
        }
        if (loadLeaderboard) PopulateLeaderboard();
    }

    private IEnumerator LoginRoutine()
    {
        bool done = false;
        LootLockerSDKManager.StartGuestSession(response =>
        {
            if (response.success)
            {
                InGameLogger.Log("Player was authenticated successfully!", Color.green);
                PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
                isAuthenticated = true;
            }
            else
            {
                InGameLogger.Log("Servers failed to authenticate player!", Color.red);
                isAuthenticated = false;
            }
            done = true;
        });
        yield return new WaitWhile(() => !done);
    }

    private void PopulateLeaderboard()
    {
        //Loading animation
        loadingAnimation.parent.gameObject.SetActive(true);
        loadingAnimation.DORotate(new Vector3(0, 0, 360), loadingAnimationSpeed, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
        
        HelperFunctions.DestroyChildren(leaderboardEntriesContainer);
        LootLockerSDKManager.GetScoreList(leaderboardID.ToString(), 9, response =>
        {
            if (response.success)
            {
                StopLoadingAnimation();
                if (response.items == null)
                {
                    return;
                }
                LootLockerLeaderboardMember[] members = response.items;
                for (int i = 0; i < members.Length; i++)
                {
                    Debug.Log("Added entry: " + i + ", " + members[i].player.name + members[i].score);
                    LeaderboardEntry leaderboardEntry = Instantiate(entryPrefab, leaderboardEntriesContainer).GetComponent<LeaderboardEntry>();
                    leaderboardEntry.SetupEntry(members[i].rank, members[i].player.name, members[i].score);
                }
            }
        });
        
        leaderboardLoaded?.Invoke();
    }

    #region Animations

    private void StartLoadingAnimation()
    {
        loadingAnimation.parent.gameObject.SetActive(true);
        loadingAnimation.DORotate(new Vector3(0, 0, 360), loadingAnimationSpeed, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
    }

    private void StopLoadingAnimation()
    {
        loadingAnimation.DOKill();
        loadingAnimation.parent.gameObject.SetActive(false);
    }

    #endregion

    #region Submit Logic

    public void SubmitTimeButton()
    {
        if (timeIntField.intField <= 0)
        {
            DisplayMessage("Go play the game first (:", Color.yellow);
        } else {
            if (canSubmit)
            {
                StartCoroutine(SubmitTime());
                StartCoroutine(SubmitCooldown());
            }
            else
            {
                DisplayMessage("Please wait before submitting another run!", Color.yellow);
            } 
        }
    }
    
    private IEnumerator SubmitTime()
    {
        //Score checking
        StartCoroutine(IsBetterScore());
        yield return new WaitUntil(() => !isBetterScoreCheck);
        if (!isBetterScore)
        {
            DisplayMessage("You already have a better score on the leaderboard!", Color.yellow);
            yield break;
        }

        bool done = false;
        string playerID = PlayerPrefs.GetString("PlayerID");

        InvalidUsernameErrorMessage invalidUsernameErrorMessage = isValidName(inputField.text);

        if (invalidUsernameErrorMessage.isValid)
        {
            // Set Name
            SetName(inputField.text);

            // Set Score
            LootLockerSDKManager.SubmitScore(playerID, timeIntField.intField, leaderboardID.ToString(), response =>
            {
                DisplayMessage("Submitting entry...", Color.yellow);
                if (response.success)
                {
                    InGameLogger.Log("Successfully submitted time!", Color.green);
                    DisplayMessage("Successfully submitted time!", Color.green);
                }
                else
                {
                    InGameLogger.Log("Unable to submit time!", Color.red);
                    DisplayMessage("Unable to submit time!", Color.red);
                }
                done = true;
            });
            yield return new WaitUntil(() => done);

            yield return HelperFunctions.GetWait(0.5f);
            PopulateLeaderboard();
        }
        else
        {
            InGameLogger.Log(invalidUsernameErrorMessage.message, Color.red);
            DisplayMessage(invalidUsernameErrorMessage.message, Color.red);
        }
    }

    private IEnumerator SubmitCooldown()
    {
        canSubmit = false;
        yield return HelperFunctions.GetWait(submitCooldown);
        canSubmit = true;
    }
    
    private IEnumerator IsBetterScore()
    {
        isBetterScoreCheck = true;
        int newScore = timeIntField.intField;
        int oldScore = 0;
        string memberID = PlayerPrefs.GetString("PlayerID");

        bool done = false;
        LootLockerSDKManager.GetMemberRank(leaderboardID.ToString(), memberID, response =>
        {
            oldScore = response.score;
            done = true;
        });

        yield return new WaitUntil(() => done);

        isBetterScore = oldScore > newScore;
        if (oldScore == 0) isBetterScore = true;
        isBetterScoreCheck = false;
    }
    #endregion

    #region Name Changing

    private void SetName(string username)
    {
        LootLockerSDKManager.SetPlayerName(username, response =>
        {
            if (response.success)
            {
                InGameLogger.Log("Successfully set the player's name to: " + username, Color.green);
            }
            else
            {
                InGameLogger.Log("Could not set the player's name to: " + username, Color.red);
            }
        });
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

    #endregion

    private void DisplayMessage(string message, Color color)
    {
        errorMessageBackground.DOKill();
        errorMessageDisplay.DOKill();
        color.a = 0f;
        errorMessageDisplay.text = message;
        errorMessageDisplay.color = color;

        errorMessageBackground.DOFade(0.8f, 0.2f).OnComplete(() =>
        {
            errorMessageBackground.DOFade(0, 3f).SetDelay(4);
        });

        errorMessageDisplay.DOFade(1, 0.2f).OnComplete(() =>
        {
            errorMessageDisplay.DOFade(0, 3f).SetDelay(4);
        });
    }

    public UnityEvent leaderboardLoaded;
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
