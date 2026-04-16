using FishRunner.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using FishRunner.Events;

namespace FishRunner.Systems
{
    public class GameController : MonoBehaviour
    {
        private IRecordsManager _recordsManager = null;
        private IAnalyticService _analyticService;
        private IUIService _uIService;

        private void Start()
        {
            EventBus.Subscribe<OnRunEnded>(AnalyticRunEnd);
            EventBus.Subscribe<OnRunStarted>(AnalyticRunStart);

            _analyticService = ServiceLocator.Get<IAnalyticService>();
            _recordsManager = ServiceLocator.Get<IRecordsManager>();
            _uIService = ServiceLocator.Get<IUIService>();

            InitializeUI();

            EventBus.RaiseEvent(new OnRunStarted { });
        }

        private void AnalyticRunStart(OnRunStarted e)
        {
            _analyticService.StartRun();
            _analyticService.LogEvent(
                "run_start",
                new Dictionary<string, object>
                {
                { "session_id", _analyticService.SessionId },
                { "best_distance", _recordsManager.ScoreData.playerData.distance },
                { "best_food", (_recordsManager.ScoreData.playerData.score -
                _recordsManager.ScoreData.playerData.distance) / 10},
                { "run_number", _analyticService.RunId }
                }
            );
        }

        private void AnalyticRunEnd(OnRunEnded e)
        {
            _analyticService.LogEvent(
                "run_end",
                new Dictionary<string, object>
                {
                { "session_id", _analyticService.SessionId },
                { "best_distance", _recordsManager.ScoreData.playerData.distance },
                { "best_food", (_recordsManager.ScoreData.playerData.score -
                _recordsManager.ScoreData.playerData.distance) / 10},
                { "result_distance",e.Distance},
                { "result_food", e.Food }
                }
            );
        }

        private void InitializeUI()
        {
            _uIService.InstantiateWidnowPause();
            _uIService.InstantiateWidnowResult();
            _uIService.InstantiatePlayerUI();
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<OnRunEnded>(AnalyticRunEnd);

            EventBus.Unsubscribe<OnRunStarted>(AnalyticRunStart);
        }
    }
}