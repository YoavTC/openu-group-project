using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InGameLogger
{
    public delegate void LogEventHandler(string message, Color color);
    public static event LogEventHandler OnLog;

    public static void Log(string message, Color color)
    {
        OnLog?.Invoke(message, color);
        Debug.Log("[IGL] " + message);
    }
    
    public static void Log(string message)
    {
        OnLog?.Invoke(message, Color.white);
        Debug.Log("[IGL] " + message);
    }
}
