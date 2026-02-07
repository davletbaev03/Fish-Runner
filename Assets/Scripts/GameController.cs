using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Spine.Unity.Examples.SpineboyFootplanter;

public class GameController : MonoBehaviour
{
    [SerializeField] private PlayerControl _player = null;
    [SerializeField] private WindowResults _windowResults = null;
    [SerializeField] private WindowPlayerUI _windowPlayerUI = null;

    [SerializeField] private RecordsManager _recordsManager = null;

    private void Start()
    {
        _player.OnGameEnd += ShowWindowResults;

        EventBus.OnRunStarted += AnalyticRunStart;
    }

    private void AnalyticRunStart()
    {
        AnalyticManager.StartRun();
        AnalyticManager.LogEvent(
            "run_start",
            new Dictionary<string, object>
            {
                { "session_id", AnalyticManager.SessionId },
                { "best_distance", _recordsManager._scoreData.playerData.distance },
                { "best_food", (_recordsManager._scoreData.playerData.score -
                _recordsManager._scoreData.playerData.distance) / 10},
                { "run_number", AnalyticManager.RunId }
            }
        );
    }

    private void AnalyticRunEnd(int distance, int food)
    {
        AnalyticManager.LogEvent(
            "run_end",
            new Dictionary<string, object>
            {
                { "session_id", AnalyticManager.SessionId },
                { "best_distance", _recordsManager._scoreData.playerData.distance },
                { "best_food", (_recordsManager._scoreData.playerData.score -
                _recordsManager._scoreData.playerData.distance) / 10},
                { "result_distance",distance},
                { "result_food", food }
            }
        );
    }

    private void ShowWindowResults(int food, int distance)
    {
        distance = Mathf.FloorToInt(distance);
        _windowPlayerUI.gameObject.SetActive(false);
        _windowResults.gameObject.SetActive(true);

        _windowResults._foodText.text = "Food: " + food;
        _windowResults._distanceText.text = "Distance: " + distance;
        _windowResults._scoreText.text = "Total Score: " + (food * 10 + distance);

        CheckRecord(_recordsManager._scoreData.playerData.name, 
            food * 10 + distance, distance);

        AnalyticRunEnd(distance, food);
    }

    private void CheckRecord(string name, int score, int distance)
    {
        _recordsManager.AddScore(name, score, distance);

        _windowResults._newRecordText.gameObject.SetActive(
            _recordsManager.TrySetNewPersonalRecord(name,score, distance));
    }
    private void OnDestroy()
    {
        _player.OnGameEnd -= ShowWindowResults;

        EventBus.OnRunStarted -= AnalyticRunStart;
    }
}
