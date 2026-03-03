using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class WindowPause : MonoBehaviour
{
    [SerializeField] private Button _buttonUnPause = null;
    [SerializeField] private Button _buttonMainMenu = null;

    [SerializeField] public CanvasGroup _darkOverlay = null;
    [SerializeField] private TextMeshProUGUI _timerText = null;

    void Start()
    {
        _buttonUnPause.onClick.AddListener(CloseWindowPause);
        _buttonMainMenu.onClick.AddListener(GoToMainMenu);

        EventBus.OnRunPaused += ShowWindowPause;

        this.gameObject.SetActive(false);
    }

    private void CloseWindowPause()
    {
        _timerText.gameObject.SetActive(true);

        _darkOverlay.blocksRaycasts = false;

        StartReadyTimer();

        _darkOverlay.DOFade(0f, 0.25f);
        this.gameObject.SetActive(false);
    }

    private void ShowWindowPause()
    {
        this.gameObject.SetActive(true);
        _darkOverlay.blocksRaycasts = true;
        _darkOverlay.DOFade(0.6f, 0.25f).SetUpdate(true);
    }

    private void StartReadyTimer()
    {
        float timerValue = 3f;

        DOTween.To(() => timerValue, x => timerValue = x, 0f, 3f)
                .SetEase(Ease.Linear)
               .SetUpdate(true)
               .OnUpdate(() =>
               {
                   _timerText.text = Mathf.Floor(timerValue + 1).ToString();
               })
               .OnComplete(() =>
               {
                   _timerText.text = "Go!";

                   _timerText.gameObject.SetActive(false);
                   Time.timeScale = 1f;
               });
    }

    private void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void OnDestroy()
    {
        EventBus.OnRunPaused -= ShowWindowPause;
    }
}
