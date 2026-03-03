using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{

    [SerializeField] private RecordsManager _recordsManager = null;

    private IAnalyticService _analyticService;
    private void Start()
    {
        EventBus.OnRunEnded += AnalyticRunEnd;
        EventBus.OnRunStarted += AnalyticRunStart;

        _analyticService = ServiceLocator.Get<IAnalyticService>();
    }

    private void AnalyticRunStart()
    {
        _analyticService.StartRun();
        _analyticService.LogEvent(
            "run_start",
            new Dictionary<string, object>
            {
                { "session_id", _analyticService.SessionId },
                { "best_distance", _recordsManager.scoreData.playerData.distance },
                { "best_food", (_recordsManager.scoreData.playerData.score -
                _recordsManager.scoreData.playerData.distance) / 10},
                { "run_number", _analyticService.RunId }
            }
        );
    }

    private void AnalyticRunEnd(int distance, int food)
    {
        _analyticService.LogEvent(
            "run_end",
            new Dictionary<string, object>
            {
                { "session_id", _analyticService.SessionId },
                { "best_distance", _recordsManager.scoreData.playerData.distance },
                { "best_food", (_recordsManager.scoreData.playerData.score -
                _recordsManager.scoreData.playerData.distance) / 10},
                { "result_distance",distance},
                { "result_food", food }
            }
        );
    }

    private void OnDestroy()
    {
        EventBus.OnRunEnded -= AnalyticRunEnd;

        EventBus.OnRunStarted -= AnalyticRunStart;
    }
}
