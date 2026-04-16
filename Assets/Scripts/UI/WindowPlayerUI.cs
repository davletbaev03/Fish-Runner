using DG.Tweening;
using FishRunner.Events;
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

        private void Awake()
        {
            _player = ServiceLocator.Get<IPlayerService>();
            _recordsManager = ServiceLocator.Get<IRecordsManager>();

            Systems.EventBus.Subscribe<OnRunStarted>(ProgressBarShow);
            Systems.EventBus.Subscribe<OnRunEnded>(Deactivation);

            Systems.EventBus.Subscribe<OnRunPaused>(_ =>
            {
                this.gameObject.SetActive(false);
            });

            Systems.EventBus.Subscribe<OnRunUnpaused> (_ =>
            {
                this.gameObject.SetActive(true);
            });

            Systems.EventBus.Subscribe<OnHealthChanged>(UpdateHealth);
            Systems.EventBus.Subscribe<OnPointsChanged>(UpdateScore);

            _buttonPause.onClick.AddListener(ShowWindowPause);
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
            
            _slider.transform.localScale = new Vector3(progress * 40, 1f, 1f);
        }
        private void UpdateHealth(OnHealthChanged e)
        {
            if (e.InstantDeath)
                foreach (var hp in _playerHealth)
                    hp.GetComponent<SpriteRenderer>().sprite = _lostHealth;
            else
                _playerHealth[e.HealthPoints].GetComponent<SpriteRenderer>().sprite = _lostHealth;
        }

        private void ProgressBarShow(OnRunStarted e)
        {
            if (_recordsManager.ScoreData.playerData.distance != 0)
                _personalBest = _recordsManager.ScoreData.playerData.distance;
            else
                _recordUI.gameObject.SetActive(false);
        }

        private void UpdateScore(OnPointsChanged e)
        {
            _foodText.text = e.Points.ToString();
        }

        private void ShowWindowPause()
        {
            Systems.EventBus.RaiseEvent(new OnRunPaused { });

            Time.timeScale = 0f;
            Systems.EventBus.RaiseEvent(new ChangeSkeletonAnim 
            { 
                Anim1 = "Swim_Normal",
                Anim2 = "Idle" 
            });

            this.gameObject.SetActive(false);
        }

        private void Deactivation(OnRunEnded e)
        {
            this.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            Systems.EventBus.Unsubscribe<OnRunStarted>(ProgressBarShow);
            Systems.EventBus.Unsubscribe<OnRunEnded>(Deactivation);
            Systems.EventBus.Unsubscribe<OnHealthChanged>(UpdateHealth);
            Systems.EventBus.Unsubscribe<OnPointsChanged>(UpdateScore);

            Systems.EventBus.Unsubscribe<OnRunUnpaused>(_ =>
            {
                this.gameObject.SetActive(true);
            });
        }
    }
}