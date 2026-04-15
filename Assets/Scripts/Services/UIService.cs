using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishRunner.UI;
using TMPro;

namespace FishRunner.Services
{
    public class UIService : IUIService
    {
        private Transform _canvas;

        private WindowResults _resultWindowPrefab;
        private WindowPause _pauseWindowPrefab;
        private TextMeshProUGUI _textReadyTimer;
        private CanvasGroup _panelDarkOverlay;
        private WindowPlayerUI _playerUIPrefab;

        public UIService(Transform canvas, WindowResults result, WindowPause pause,
            TextMeshProUGUI textReadyTimer, CanvasGroup panelDarkOverlay,
            WindowPlayerUI playerUIPrefab)
        {
            _canvas = canvas;
            _resultWindowPrefab = result;
            _pauseWindowPrefab = pause;
            _textReadyTimer = textReadyTimer;
            _panelDarkOverlay = panelDarkOverlay;
            _playerUIPrefab = playerUIPrefab;
        }

        public void InstantiateWidnowPause()
        {
            var overlay = Object.Instantiate(_panelDarkOverlay, _canvas);
            var pauseWindow = Object.Instantiate(_pauseWindowPrefab, _canvas);
            var timer = Object.Instantiate(_textReadyTimer, _canvas);

            pauseWindow.DarkOverlay = overlay;
            pauseWindow.TimerText = timer;

            pauseWindow.DarkOverlay.gameObject.SetActive(true);
            pauseWindow.TimerText.gameObject.SetActive(false);
        }

        public void InstantiateWidnowResult()
        {
            Object.Instantiate(_resultWindowPrefab, _canvas);
        }
        
        public void InstantiatePlayerUI()
        {
            Object.Instantiate(_playerUIPrefab, _canvas);
        }
    }
}