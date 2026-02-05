using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class WindowMainMenu : MonoBehaviour
{
    [SerializeField] private Button _startButton = null;
    [SerializeField] private Button _recordsButton = null;
    [SerializeField] private Button _settingsButton = null;

    [SerializeField] private WindowRecords _windowRecords;
    [SerializeField] private WindowSettings _windowSettings;

    
    private void OnApplicationFocus(bool focus)
    {
        AnalyticManager.StartSession();

        AnalyticManager.LogEvent(
            "app_start",
            new Dictionary<string, object>
            {
            { "session_id", AnalyticManager.SessionId },
            { "best_distance", _windowRecords._recordsManager._scoreData.playerData.distance },
            { "best_food", (_windowRecords._recordsManager._scoreData.playerData.score - 
            _windowRecords._recordsManager._scoreData.playerData.distance) / 10}
            }
        );
    }
    void Start()
    {
        _startButton.onClick.AddListener(GameLaunch);
        _recordsButton.onClick.AddListener(ShowRecords);
        _settingsButton.onClick.AddListener(ShowSettings);
    }

    private void GameLaunch()
    {
        SceneManager.LoadScene("Game");
    }

    private void ShowRecords()
    {
        _windowRecords.gameObject.SetActive(true);
    }
    private void ShowSettings()
    {
        _windowSettings.gameObject.SetActive(true);
    }
}
