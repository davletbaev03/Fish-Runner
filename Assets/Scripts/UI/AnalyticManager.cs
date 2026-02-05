using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnalyticManager
{
    public static string SessionId { get; private set; }
    public static string RunId { get; private set; }

    public static void StartSession()
    {
        SessionId = System.Guid.NewGuid().ToString();
    }

    public static void StartRun()
    {
        RunId = System.Guid.NewGuid().ToString();
    }

    public static void LogEvent(string eventName, Dictionary<string, object> data)
    {
        Debug.Log($"[ANALYTICS] {eventName}");

        foreach (var pair in data)
            Debug.Log($"{pair.Key}: {pair.Value}");
    }
}
