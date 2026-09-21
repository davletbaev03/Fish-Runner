using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using FishRunner.Systems;
using FishRunner.Configs;

namespace FishRunner.UI
{
    public class WindowPause : MonoBehaviour, IUIObject
    {
        public string Id => nameof(WindowPause);

        [SerializeField] private Button _buttonUnPause = null;
        [SerializeField] private Button _buttonMainMenu = null;

        public Image image = null;
        [SerializeField] public CanvasGroup DarkOverlay = null;
        [SerializeField] public TextMeshProUGUI TimerText = null;

        void Start()
        {
            image = this.GetComponent<Image>();
            _buttonUnPause.onClick.AddListener(Hide);
            _buttonMainMenu.onClick.AddListener(GoToMainMenu);

            EventBus.Subscribe<OnRunPaused>(ShowWindowPause);

            this.gameObject.SetActive(false);
        }

        public void Hide()
        {
            image.color = new Color(
                image.color.r,
                image.color.g,
                image.color.b,
                0f
            );
            _buttonMainMenu.gameObject.SetActive(false);
            _buttonUnPause.gameObject.SetActive(false);
            TimerText.gameObject.SetActive(true);

            DarkOverlay.blocksRaycasts = false;
            DarkOverlay.DOFade(0f, 0.25f);

            StartReadyTimer();
        }

        private void ShowWindowPause(OnRunPaused e)
        {
            Show();
        }

        public void Show()
        {
            this.gameObject.SetActive(true);

            image.color = new Color(
                image.color.r,
                image.color.g,
                image.color.b,
                1f
            );
            _buttonMainMenu.gameObject.SetActive(true);
            _buttonUnPause.gameObject.SetActive(true);

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
                       this.gameObject.SetActive(false);
                       Time.timeScale = 1f;

                   EventBus.RaiseEvent(new OnRunUnpaused { });
                   });
        }

        private void GoToMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<OnRunPaused>(ShowWindowPause);
        }
    }
}
