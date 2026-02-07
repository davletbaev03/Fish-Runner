using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnalyticManager
{
    public static string SessionId { get; private set; }
    public static string RunId { get; private set; }

    private const string SessionCounterKey = "SessionCounter";
    private const string RunCounterKey = "RunCounter";

    public static void StartSession()
    {
        int counter = PlayerPrefs.GetInt(SessionCounterKey, 0);
        counter++;

        PlayerPrefs.SetInt(SessionCounterKey, counter);
        PlayerPrefs.Save();

        SessionId = $"session_{counter}";
    }

    public static void StartRun()
    {
        int counter = PlayerPrefs.GetInt(RunCounterKey, 0);
        counter++;

        PlayerPrefs.SetInt(RunCounterKey, counter);
        PlayerPrefs.Save();

        RunId = $"run_{counter}";
    }

    public static void LogEvent(string eventName, Dictionary<string, object> data)
    {
        Debug.Log($"[ANALYTICS] {eventName}");

        foreach (var pair in data)
            Debug.Log($"{pair.Key}: {pair.Value}");
    }
}
