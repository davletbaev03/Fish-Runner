using DG.Tweening;
using FishRunner.Services;
using FishRunner.Systems;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace FishRunner.UI
{
    public class WindowPlayerUI : MonoBehaviour
    {
        [SerializeField] private Button _buttonPause = null;

        [SerializeField] private TextMeshProUGUI _foodText = null;

        [SerializeField] IPlayerService _player;

        [SerializeField] private List<GameObject> _playerHealth;
        [SerializeField] private Sprite _lostHealth;

        [SerializeField] private GameObject _recordUI = null;
        private IRecordsManager _recordsManager = null;
        [SerializeField] private GameObject _slider = null;
        private float _personalBest = 0;

        private void Start()
        {
            _player = ServiceLocator.Get<IPlayerService>();
            Systems.EventBus.OnRunStarted += ProgressBarShow;
            Systems.EventBus.OnRunEnded += Deactivation;

            Systems.EventBus.OnRunPaused += () =>
            {
                this.gameObject.SetActive(false);
            };

            Systems.EventBus.OnRunUnpaused += () =>
            {
                this.gameObject.SetActive(true);
            };

            Systems.EventBus.OnHealthChanged += UpdateHealth;
            Systems.EventBus.OnPointsChanged += UpdateScore;

            _buttonPause.onClick.AddListener(ShowWindowPause);

            _recordsManager = ServiceLocator.Get<IRecordsManager>();
        }

        private void Update()
        {
            if (_recordUI == null || _personalBest == 0)
                return;

            if (Mathf.FloorToInt(_player.Position.x) > _personalBest)
            {
                Destroy(_recordUI);
                _recordUI = null;
                return;
            }

            float progress = _player.Position.x / _personalBest;
            progress = Mathf.Clamp01(progress);

            _slider.transform.localScale = new Vector3(progress, 1f, 1f);
        }
        private void UpdateHealth(int healthPoints, bool instantDeath)
        {
            if (instantDeath)
                foreach (var hp in _playerHealth)
                    hp.GetComponent<SpriteRenderer>().sprite = _lostHealth;
            else
                _playerHealth[healthPoints].GetComponent<SpriteRenderer>().sprite = _lostHealth;
        }

        private void ProgressBarShow()
        {
            if (_recordsManager.ScoreData.playerData.distance != 0)
                _personalBest = _recordsManager.ScoreData.playerData.distance;
            else
                _recordUI.gameObject.SetActive(false);
        }

        private void UpdateScore(int score)
        {
            _foodText.text = score.ToString();
        }

        private void ShowWindowPause()
        {
            Systems.EventBus.OnRunPaused?.Invoke();

            Time.timeScale = 0f;
            Systems.EventBus.ChangeSkeletonAnim("Swim_Normal", "Idle");

            this.gameObject.SetActive(false);
        }

        private void Deactivation(int a, int b)
        {
            this.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            Systems.EventBus.OnRunStarted -= ProgressBarShow;
            Systems.EventBus.OnRunEnded -= Deactivation;
            Systems.EventBus.OnHealthChanged -= UpdateHealth;
            Systems.EventBus.OnPointsChanged -= UpdateScore;
        }
    }
}