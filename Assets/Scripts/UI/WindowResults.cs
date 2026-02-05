using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WindowResults : MonoBehaviour
{
    [SerializeField] private Button _buttonRestart = null;
    [SerializeField] private Button _buttonMainMenu = null;

    [SerializeField] public TextMeshProUGUI _foodText = null;
    [SerializeField] public TextMeshProUGUI _distanceText = null;
    [SerializeField] public TextMeshProUGUI _scoreText = null;
    [SerializeField] public TextMeshProUGUI _newRecordText = null;

    private Tween _pulseTween;

    void Start()
    {
        _buttonRestart.onClick.AddListener(RestartGame);
        _buttonMainMenu.onClick.AddListener(GoToMainMenu);
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

    private void OnDisable()
    {
        _pulseTween?.Kill();
    }
}
