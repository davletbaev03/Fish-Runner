using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnalyticService
{
    public void StartSession();
    public void StartRun();
    public void LogEvent(string eventName, Dictionary<string, object> data);

    public string SessionId { get; }
    public string RunId { get; }
}

public class AnalyticService : IAnalyticService
{
    public string SessionId { get; private set; }
    public string RunId { get; private set; }

    private const string SessionCounterKey = "SessionCounter";
    private const string RunCounterKey = "RunCounter";

    public void StartSession()
    {
        int counter = PlayerPrefs.GetInt(SessionCounterKey, 0);
        counter++;

        PlayerPrefs.SetInt(SessionCounterKey, counter);
        PlayerPrefs.Save();

        SessionId = $"session_{counter}";
    }

    public void StartRun()
    {
        int counter = PlayerPrefs.GetInt(RunCounterKey, 0);
        counter++;

        PlayerPrefs.SetInt(RunCounterKey, counter);
        PlayerPrefs.Save();

        RunId = $"run_{counter}";
    }

    public void LogEvent(string eventName, Dictionary<string, object> data)
    {
        Debug.Log($"[ANALYTICS] {eventName}");

        foreach (var pair in data)
            Debug.Log($"{pair.Key}: {pair.Value}");
    }
}
