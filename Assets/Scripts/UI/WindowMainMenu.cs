using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using FishRunner.Services;
using Cysharp.Threading.Tasks;
using Zenject;

namespace FishRunner.UI
{
    public class WindowMainMenu : MonoBehaviour, IUIObject
    {
        public string Id => nameof(WindowMainMenu);

        [SerializeField] private Button _startButton = null;
        [SerializeField] private Button _recordsButton = null;
        [SerializeField] private Button _settingsButton = null;

        [SerializeField] private WindowRecords _windowRecords;
        [SerializeField] private WindowSettings _windowSettings;

        private IAnalyticService _analyticService;
        private ILoadingService _loadingService;

        [Inject]
        private void Construct(IAnalyticService analyticService, ILoadingService loadingService)
        {
            _analyticService = analyticService;
            _loadingService = loadingService;
        }
        private void Awake()
        {
            _startButton.onClick.AddListener(GameLaunch);
            _recordsButton.onClick.AddListener(ShowRecords);
            _settingsButton.onClick.AddListener(ShowSettings);

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
            LoadGame().Forget();
        }

        private async UniTask LoadGame()
        {
            await _loadingService.LoadScene("Game");
        }

        private void ShowRecords()
        {
            _windowRecords.Show();
        }
        private void ShowSettings()
        {
            _windowSettings.Show();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}