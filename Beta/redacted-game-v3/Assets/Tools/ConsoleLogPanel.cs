using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleLogPanel : MonoBehaviour
{
   [SerializeField] private GameObject consoleLogPrefab;
   [SerializeField] private bool isConsolePanelEnabled; 

    private void OnEnable() => InGameLogger.OnLog += HandleLog;
    private void OnDisable() => InGameLogger.OnLog -= HandleLog;

    private void Update()
    {
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.L))
        {
            isConsolePanelEnabled = !isConsolePanelEnabled;
        }
        #else
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.L))
        {
            isConsolePanelEnabled = !isConsolePanelEnabled;
        }
        #endif
    }
    
    private void HandleLog(string message, Color color)
    {
        if (!isConsolePanelEnabled) return;
        TMP_Text newLog = Instantiate(consoleLogPrefab, transform).GetComponent<TMP_Text>();
        newLog.color = color;
        newLog.text = message;
        
        Debug.Log("InGameLogger: " + message);

        StartCoroutine(KillMessage(newLog));
    }

    private IEnumerator KillMessage(TMP_Text message)
    {
        yield return HelperFunctions.GetWait(5f);
        message.DOFade(0, 0.5f).OnComplete(() =>
        {
            Destroy(message.gameObject);
        });
    }
}
