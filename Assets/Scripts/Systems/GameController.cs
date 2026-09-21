using FishRunner.Services;
using FishRunner.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace FishRunner.Systems
{
    public class GameController : MonoBehaviour
    {
        private IRecordsManager _recordsManager = null;
        private IAnalyticService _analyticService = null;
        private IUIService _uIService = null;

        private void Awake()
        {
            Debug.Log("GAME CONTROLLER AWAKE");
        }

        [Inject]
        private void Construct(IRecordsManager recordsManager,IAnalyticService analyticService,
            IUIService uiService)
        {
            Debug.Log("CONSTRUCT GAME CONTROLLER");
            _recordsManager = recordsManager;
            _analyticService = analyticService;
            _uIService = uiService;
        }

        private void Start()
        {
            EventBus.Subscribe<OnRunEnded>(AnalyticRunEnd);
            EventBus.Subscribe<OnRunStarted>(AnalyticRunStart);

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
            _uIService.Instantiate(nameof(WindowPause));
            _uIService.Instantiate(nameof(WindowResults));
            _uIService.Instantiate(nameof(WindowPlayerUI));
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<OnRunEnded>(AnalyticRunEnd);

            EventBus.Unsubscribe<OnRunStarted>(AnalyticRunStart);
        }
    }
}