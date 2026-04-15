using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using FishRunner.Systems;

namespace FishRunner.UI
{
    public class WindowPause : MonoBehaviour
    {
        [SerializeField] private Button _buttonUnPause = null;
        [SerializeField] private Button _buttonMainMenu = null;

        public CanvasGroup DarkOverlay = null;
        public TextMeshProUGUI TimerText = null;

        void Start()
        {
            _buttonUnPause.onClick.AddListener(CloseWindowPause);
            _buttonMainMenu.onClick.AddListener(GoToMainMenu);

            Systems.EventBus.OnRunPaused += ShowWindowPause;

            this.gameObject.SetActive(false);
        }

        private void CloseWindowPause()
        {
            TimerText.gameObject.SetActive(true);

            DarkOverlay.blocksRaycasts = false;
            DarkOverlay.DOFade(0f, 0.25f);

            StartReadyTimer();
            
            this.gameObject.SetActive(false);

            
        }

        private void ShowWindowPause()
        {
            this.gameObject.SetActive(true);
            DarkOverlay.blocksRaycasts = true;
            DarkOverlay.DOFade(0.6f, 0.25f).SetUpdate(true);
        }

        private void StartReadyTimer()
        {
            float timerValue = 3f;

            DOTween.To(() => timerValue, x => timerValue = x, 0f, 3f)
                    .SetEase(Ease.Linear)
                   .SetUpdate(true)
                   .OnUpdate(() =>
                   {
                       TimerText.text = Mathf.Floor(timerValue + 1).ToString();
                   })
                   .OnComplete(() =>
                   {
                       TimerText.text = "Go!";

                       TimerText.gameObject.SetActive(false);
                       Time.timeScale = 1f;

                       Systems.EventBus.OnRunUnpaused?.Invoke();
                   });
        }

        private void GoToMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        private void OnDestroy()
        {
            Systems.EventBus.OnRunPaused -= ShowWindowPause;
        }
    }
}
