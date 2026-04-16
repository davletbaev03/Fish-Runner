using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using FishRunner.Services;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace FishRunner.UI
{
    public class WindowMainMenu : MonoBehaviour
    {
        [SerializeField] private Button _startButton = null;
        [SerializeField] private Button _recordsButton = null;
        [SerializeField] private Button _settingsButton = null;

        [SerializeField] private WindowRecords _windowRecords;
        [SerializeField] private WindowSettings _windowSettings;

        private IAnalyticService _analyticService;

        private void Awake()
        {
            _startButton.onClick.AddListener(GameLaunch);
            _recordsButton.onClick.AddListener(ShowRecords);
            _settingsButton.onClick.AddListener(ShowSettings);

            _analyticService = ServiceLocator.Get<IAnalyticService>();
        }
        private void Start()
        {
            AnalyticAppStart();
        }

        private void AnalyticAppStart()
        {
            _analyticService.StartSession();

            _analyticService.LogEvent(
                "app_start",
                new Dictionary<string, object>
                {
            { "session_id", _analyticService.SessionId },
            { "best_distance", Mathf.FloorToInt(_windowRecords._recordsManager.ScoreData.playerData.distance) },
            { "best_food", (_windowRecords._recordsManager.ScoreData.playerData.score -
            Mathf.FloorToInt(_windowRecords._recordsManager.ScoreData.playerData.distance)) / 10}
                }
            );
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
}