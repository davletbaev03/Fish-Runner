using FishRunner.Configs;
using FishRunner.Player;
using FishRunner.Services;
using FishRunner.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FishRunner.Systems
{
    public class Bootstrap : MonoBehaviour
    {
        [Header ("UI")]
        [SerializeField] private Transform _canvas;
        [SerializeField] private WindowResults _resultWindowPrefab;
        [SerializeField] private WindowPause _pauseWindowPrefab;
        [SerializeField] private TextMeshProUGUI _textReadyTimer;
        [SerializeField] private CanvasGroup _panelDarkOverlay;
        [SerializeField] private WindowPlayerUI _playerUIPrefab;

        [Header("Player")]
        [SerializeField] private PlayerControl _player;

        [Header("Player Audio")]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private SoundConfig _moveSideClip;
        [SerializeField] private SoundConfig _deathClip;

        private void Awake()
        {
            var UIService = new UIService(_canvas, _resultWindowPrefab, _pauseWindowPrefab,
                _textReadyTimer, _panelDarkOverlay, _playerUIPrefab);
            ServiceLocator.Register<IUIService>(UIService);

            var playerService = new PlayerService(_player);
            ServiceLocator.Register<IPlayerService>(playerService);

            var playerAudioService = new PlayerAudioService(_audioSource, _moveSideClip, _deathClip);
            ServiceLocator.Register<IPlayerAudioService>(playerAudioService);

            if (!ServiceLocator.Services.ContainsKey(typeof(ISaveLoadService)))
            {
                var saveLoadService = new SaveLoadService();
                ServiceLocator.Register<ISaveLoadService>(saveLoadService);

                var analyticService = new AnalyticService();
                ServiceLocator.Register<IAnalyticService>(analyticService);

                var recordsManager = new RecordsManager();
                ServiceLocator.Register<IRecordsManager>(recordsManager);
            }
            
        }
    }
}
