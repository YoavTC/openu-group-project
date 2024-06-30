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
    }
    
    public static void Log(string message)
    {
        OnLog?.Invoke(message, Color.white);
    }
}
