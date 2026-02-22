using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WindowPlayerUI : MonoBehaviour
{
    [SerializeField] private Button _buttonPause = null;
    [SerializeField] private WindowPause _windowPause = null;

    [SerializeField] private TextMeshProUGUI _foodText = null;

    [SerializeField] IPlayerService _player;

    [SerializeField] private List<GameObject> _playerHealth;
    [SerializeField] private Sprite _lostHealth;

    [SerializeField] private GameObject _recordUI = null;
    [SerializeField] private RecordsManager _recordsManager = null;
    [SerializeField] private GameObject _slider = null;
    private float _personalBest = 0;

    private void Start()
    {
        _player = ServiceLocator.Get<IPlayerService>();
        EventBus.OnRunStarted += ProgressBarShow;

        EventBus.OnHealthChanged += UpdateHealth;
        EventBus.OnPointsChanged += UpdateScore;

        _buttonPause.onClick.AddListener(ShowWindowPause);
    }

    private void Update()
    {
        if (_recordUI == null || _personalBest == 0)
            return;
        if (Mathf.FloorToInt(_player.Position.x) > _personalBest)
        {
            Destroy(_recordUI);
        }
        _slider.transform.localScale
            = new Vector3(40 * _player.Position.x/_personalBest, 1,0);
    }
    private void UpdateHealth(int healthPoints, bool instantDeath)
    {
        if(instantDeath)
            foreach(var hp in _playerHealth)
                hp.GetComponent<SpriteRenderer>().sprite = _lostHealth;
        else
            _playerHealth[healthPoints].GetComponent<SpriteRenderer>().sprite = _lostHealth;
    }

    private void ProgressBarShow()
    {
        if (_recordsManager._scoreData.playerData.distance != 0)
            _personalBest = _recordsManager._scoreData.playerData.distance;
        else
            _recordUI.gameObject.SetActive(false);
    }

    private void UpdateScore(int score)
    {
        _foodText.text = score.ToString();
    }

    private void ShowWindowPause()
    {
        Time.timeScale = 0f;
        EventBus.ChangeSkeletonAnim("Swim_Normal", "Idle");
        _windowPause.gameObject.SetActive(true);
        this.gameObject.SetActive(false);

        _windowPause._darkOverlay.blocksRaycasts = true;
        _windowPause._darkOverlay.DOFade(0.6f, 0.25f).SetUpdate(true);
    }

    private void OnDestroy()
    {
        EventBus.OnRunStarted -= ProgressBarShow;
        EventBus.OnHealthChanged -= UpdateHealth;
        EventBus.OnPointsChanged -= UpdateScore;
    }
}
