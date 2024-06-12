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
        string version = GetLastShortCommitId() + "_d" + DateTime.Today.DayOfYear;
        GetComponent<TMP_Text>().text = prefix + version;
    }

    private string GetLastShortCommitId()
    {
        ProcessStartInfo processStartInfo = new ProcessStartInfo("git", "rev-parse --short HEAD")
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

        if (!string.IsNullOrEmpty(output))
        {
            return output.Trim();
        }
        Debug.Log("Failed to retrieve last short commit ID.");
        return "unknown";
    }
}
