using DG.Tweening;
using FishRunner.Configs;
using FishRunner.Services;
using FishRunner.Systems;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using Zenject;

namespace FishRunner.UI
{
    public class WindowResults : MonoBehaviour, IUIObject
    {
        public string Id => nameof(WindowResults);

        [SerializeField] private Button _buttonRestart = null;
        [SerializeField] private Button _buttonMainMenu = null;

        [SerializeField] public TextMeshProUGUI _foodText = null;
        [SerializeField] public TextMeshProUGUI _distanceText = null;
        [SerializeField] public TextMeshProUGUI _scoreText = null;
        [SerializeField] public TextMeshProUGUI _newRecordText = null;

        private IRecordsManager _recordsManager = null;

        private Tween _pulseTween;

        [Inject]
        private void Construct(IRecordsManager recordsManager)
        {
            _recordsManager = recordsManager;
        }

        void Start()
        {

            Systems.EventBus.Subscribe<OnRunEnded>(ShowWindowResults);

            _buttonRestart.onClick.AddListener(RestartGame);
            _buttonMainMenu.onClick.AddListener(GoToMainMenu);

            Hide();
        }
        private void OnEnable()
        {
            _pulseTween = _newRecordText.transform
                .DOScale(1.1f, 0.4f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine)
                .SetUpdate(true);
        }

        private void RestartGame()
        {
            SceneManager.LoadScene("Game");
        }

        private void GoToMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        private void ShowWindowResults(OnRunEnded e)
        {
            Show();
            e.Distance = Mathf.FloorToInt(e.Distance);

            _foodText.text = "Food: " + e.Food;
            _distanceText.text = "Distance: " + e.Distance;
            _scoreText.text = "Total Score: " + (e.Food * 10 + e.Distance);

            _recordsManager.AddScore(_recordsManager.ScoreData.playerData.name, (e.Food * 10 + e.Distance), e.Distance);

            _newRecordText.gameObject.SetActive(
                _recordsManager.TrySetNewPersonalRecord(_recordsManager.ScoreData.playerData.name
                , (e.Food * 10 + e.Distance), e.Distance));
        }


        private void OnDisable()
        {
            _pulseTween?.Kill();
        }

        private void OnDestroy()
        {
            Systems.EventBus.Unsubscribe<OnRunEnded>(ShowWindowResults);
        }

        public void Show()
        {
            this.gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}