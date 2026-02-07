using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Spine.Unity;

public class PlayerControl : MonoBehaviour
{
    private Vector2 _startPos;
    private Vector2 _endPos;

    private float _minSwipeDist = 1f;
    private bool _isMovingSide = false;
    private bool _isGameEnd = false;

    private DateTime _startIFramesTime = DateTime.Now;
    private Tween _flashSequence;

    [SerializeField] private int _points = 0;
    [SerializeField] private int _healthPoints = 3;
    [SerializeField] private float _speed = 4f;

    [SerializeField] private float _acceleration = 1.1f;

    [SerializeField] private PlayerAudio _playerAudio = null;

    [SerializeField] public SkeletonAnimation Skeleton;

    public event Action<int,bool> OnHealthChanged;

    public event Action<int> OnPointsChanged;

    public event Action<int, int> OnGameEnd;

    public float Speed
    {
        get { return _speed; }
        set { _speed = value; }
    }

    public int HealthPoints
    {
        get { return _healthPoints; }
    }

    public int Food
    {
        get { return _points; }   
    }

    private void Start()
    {
        AccelerateByTime();
        Skeleton.AnimationState.SetAnimation(0, "Swim_Normal", true);
    }

    void Update()
    {
        if (_isGameEnd)
            return;

        this.transform.Translate(Vector2.left * _speed * Time.deltaTime);
        
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
                _startPos = touch.position;

            else if (touch.phase == TouchPhase.Ended)
            {
                _endPos = touch.position;
                CheckSwipe();
            }
        }
        // Проверка для мыши (редактор / ПК)
        else if (Input.GetMouseButtonDown(0))
        {
            _startPos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _endPos = Input.mousePosition;
            CheckSwipe();
        }
    }

    private void AccelerateByTime()
    {
        DOVirtual.DelayedCall(10f, () =>
        {
            DOTween.To(() => _speed, x => _speed = x, _speed * _acceleration, 2f)
            .SetEase(Ease.OutQuad);

            if (_speed < 12f)
                AccelerateByTime();
            else
                _speed = 12f;
        });
    }

    private void CheckSwipe()
    {
        float swipeDistY = _endPos.y - _startPos.y;

        if (Mathf.Abs(swipeDistY) < _minSwipeDist)
            return;

        if (!_isMovingSide && swipeDistY > 0 && transform.position.y < 3)
        {
            _playerAudio.PlayMoveSide();

            _isMovingSide = true;
            transform.DOMoveY(transform.position.y + 2f, 0.2f).SetEase(Ease.OutQuad)
                .OnComplete(() => _isMovingSide = false);
        }
        else if (!_isMovingSide && swipeDistY < 0 && transform.position.y > -3)
        {
            _playerAudio.PlayMoveSide();

            _isMovingSide = true;
            transform.DOMoveY(transform.position.y - 2f, 0.2f).SetEase(Ease.OutQuad)
                .OnComplete(() => _isMovingSide = false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null)
            return;

        Debug.LogError("Collision");
        switch(collision.tag)
        {
            case ("Food"):
                Skeleton.AnimationState.SetAnimation(0, "Eat", false);
                Skeleton.AnimationState.AddAnimation(0, "Swim_Normal", true, 0);

                _playerAudio.PlayEat();

                Destroy(collision.gameObject);
                _points++;

                OnPointsChanged?.Invoke(_points);
                break;

            case ("Coral"):
            case ("Trash"):
                if (DateTime.Now - _startIFramesTime > TimeSpan.FromSeconds(3))
                {
                    Skeleton.AnimationState.SetAnimation(0, "Damage", false);
                    Skeleton.AnimationState.AddAnimation(0, "Swim_Normal", true, 2);

                    _healthPoints--;
                    _startIFramesTime = DateTime.Now;

                    OnHealthChanged?.Invoke(_healthPoints,false);
                }
                if (_healthPoints < 1)
                {
                    Skeleton.AnimationState.SetAnimation(0, "Death", false);
                    Skeleton.AnimationState.AddAnimation(0, "Death_Idle", true,0);

                    _playerAudio.PlayDeath();
                    _speed = 0f;

                    _isGameEnd = true;
                    OnGameEnd?.Invoke(_points, Mathf.FloorToInt(transform.position.x));
                }
                else
                {
                    _playerAudio.PlayHit();
                    IFramesGlowing(2f);
                }

                Destroy(collision.gameObject);
                //Debug.LogError($"Collision - health: {_healthPoints}");
                break;

            case ("Net"):
                OnHealthChanged?.Invoke(_healthPoints, true);

                Skeleton.AnimationState.SetAnimation(0, "Death", false);
                Skeleton.AnimationState.AddAnimation(0, "Death_Idle", true, 0);

                _speed = 0f;
                _playerAudio.PlayDeath();

                _isGameEnd = true;
                OnGameEnd?.Invoke(_points, Mathf.FloorToInt(transform.position.x));
                //Debug.LogError("Game Over");
                break;

        }
    }

    private void IFramesGlowing(float duration, float interval = 0.1f)
    {
        StopFlash();

        float elapsed = 0f;

        _flashSequence = DOTween.To(() => 0f, x =>
        {
            elapsed += Time.deltaTime;
            bool on = Mathf.FloorToInt(elapsed / interval) % 2 == 0;
            Skeleton.Skeleton.SetColor(on ? Color.white : Color.red);
        }, 1f, duration)
        .SetEase(Ease.Linear);
    }

    public void StopFlash()
    {
        _flashSequence?.Kill();
        Skeleton.Skeleton.SetColor(Color.white);
    }
}
