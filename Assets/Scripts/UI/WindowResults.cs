using DG.Tweening;
using FishRunner.Events;
using FishRunner.Services;
using FishRunner.Systems;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

namespace FishRunner.UI
{
    public class WindowResults : MonoBehaviour
    {
        [SerializeField] private Button _buttonRestart = null;
        [SerializeField] private Button _buttonMainMenu = null;

        [SerializeField] public TextMeshProUGUI _foodText = null;
        [SerializeField] public TextMeshProUGUI _distanceText = null;
        [SerializeField] public TextMeshProUGUI _scoreText = null;
        [SerializeField] public TextMeshProUGUI _newRecordText = null;

        private IRecordsManager _recordsManager = null;

        private Tween _pulseTween;

        void Start()
        {
            _recordsManager = ServiceLocator.Get<IRecordsManager>();

            EventBus.Subscribe<OnRunEnded>(ShowWindowResults);

            _buttonRestart.onClick.AddListener(RestartGame);
            _buttonMainMenu.onClick.AddListener(GoToMainMenu);

            this.gameObject.SetActive(false);
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
            this.gameObject.SetActive(true);
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
            EventBus.Unsubscribe<OnRunEnded>(ShowWindowResults);
        }
    }
}