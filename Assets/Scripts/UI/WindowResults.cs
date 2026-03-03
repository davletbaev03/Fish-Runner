using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class WindowResults : MonoBehaviour
{
    [SerializeField] private Button _buttonRestart = null;
    [SerializeField] private Button _buttonMainMenu = null;

    [SerializeField] public TextMeshProUGUI _foodText = null;
    [SerializeField] public TextMeshProUGUI _distanceText = null;
    [SerializeField] public TextMeshProUGUI _scoreText = null;
    [SerializeField] public TextMeshProUGUI _newRecordText = null;

    [SerializeField] private RecordsManager _recordsManager = null;

    private Tween _pulseTween;

    void Start()
    {
        EventBus.OnRunEnded += ShowWindowResults;

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

    private void ShowWindowResults(int food, int distance)  
    {
        this.gameObject.SetActive(true);
        distance = Mathf.FloorToInt(distance);

        _foodText.text = "Food: " + food;
        _distanceText.text = "Distance: " + distance;
        _scoreText.text = "Total Score: " + (food * 10 + distance);

        _recordsManager.AddScore(_recordsManager.scoreData.playerData.name, (food * 10 + distance), distance);

        _newRecordText.gameObject.SetActive(
            _recordsManager.TrySetNewPersonalRecord(_recordsManager.scoreData.playerData.name
            , (food * 10 + distance), distance));
    }
   

    private void OnDisable()
    {
        _pulseTween?.Kill();
    }

    private void OnDestroy()
    {
        EventBus.OnRunEnded -= ShowWindowResults;
    }
}
