using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;

[RequireComponent(typeof(TMP_Text))]
public class VersionDisplayer : MonoBehaviour
{
    [SerializeField] private string prefix;
    void Start()
    {
        string version = GetCommitCount() + "_d" + DateTime.Today.DayOfYear;
        GetComponent<TMP_Text>().text = prefix + version;
    }

    private int GetCommitCount()
    {
        ProcessStartInfo processStartInfo = new ProcessStartInfo("git", "rev-list --count HEAD")
        {
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            WorkingDirectory = Application.dataPath // Ensures the command runs in the project directory
        };
        
        Process process = new Process
        {
            StartInfo = processStartInfo
        };

        process.Start();

        string output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        process.Close();

        int commitCount = 0;
        if (int.TryParse(output.Trim(), out commitCount))
        {
            return commitCount;
        }
        Debug.Log("Failed to parse commit count.");
        return -1;
    }
}
