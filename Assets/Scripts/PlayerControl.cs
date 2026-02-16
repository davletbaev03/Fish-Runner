using DG.Tweening;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Vector2 _startPos;
    private Vector2 _endPos;

    private float _minSwipeDist = 1f;
    private bool _isMovingSide = false;
    public bool isGameEnd = false;

    private DateTime _startIFramesTime = DateTime.Now;
    private Tween _flashSequence;

    [SerializeField] private PlayerParams _playerParams;
    private float _speed;
    private int _food;
    private int _healthPoints;

    [SerializeField] private PlayerAudio _playerAudio = null;

    [SerializeField] public SkeletonAnimation Skeleton;

    public event Action<int,bool> OnHealthChanged;

    public event Action<int> OnPointsChanged;

    public float Speed
    {
        get { return _speed; }
    }

    public int HealthPoints
    {
        get { return _healthPoints; }
    }

    public int Food
    {
        get { return _food; }   
    }

    private void Start()
    {
        _speed = _playerParams.speed;
        _food = _playerParams.points;
        _healthPoints = _playerParams.healthPoints;

        AccelerateByTime();
        Skeleton.AnimationState.SetAnimation(0, "Swim_Normal", true);
    }

    void Update()
    {
        if (isGameEnd)
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
            DOTween.To(() => _speed, x => _speed = x,
                _speed * _playerParams.acceleration, 2f)
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
            _playerAudio.Play(_playerAudio.moveSideClip);

            _isMovingSide = true;
            transform.DOMoveY(transform.position.y + 2f, 0.2f).SetEase(Ease.OutQuad)
                .OnComplete(() => _isMovingSide = false);
        }
        else if (!_isMovingSide && swipeDistY < 0 && transform.position.y > -3)
        {
            _playerAudio.Play(_playerAudio.moveSideClip);

            _isMovingSide = true;
            transform.DOMoveY(transform.position.y - 2f, 0.2f).SetEase(Ease.OutQuad)
                .OnComplete(() => _isMovingSide = false);
        }
    }

    public void ApplyInteraction(PlayerInteractionConfig config, GameObject source)
    {
        if (config.addPoints != 0)
            AddPoints(config.addPoints, source);

        if (config.damage > 0)
            TakeDamage(config.damage, source);

        if (config.killInstant || _healthPoints < 1)
            PlayerDeath();

        if (!string.IsNullOrEmpty(config.animation))
            if (_healthPoints > 0)
                PlayAnimation(config.animation, config.animation2);
            else
                PlayAnimation("Death", "Death_Idle");

        if (config.sound != null)
                _playerAudio.Play(config.sound);

        if (config.destroySource)
            Destroy(source);
    }

    private void AddPoints(int points, GameObject collision)
    {
        Destroy(collision.gameObject);
        _food++;

        OnPointsChanged?.Invoke(_food);
    }

    private void TakeDamage(int damage, GameObject collision)
    {
        if (DateTime.Now - _startIFramesTime > TimeSpan.FromSeconds(3))
        {
            _healthPoints--;
            _startIFramesTime = DateTime.Now;

            OnHealthChanged?.Invoke(_healthPoints, false);
        }
        if (_healthPoints < 1)
        {
            PlayAnimation("Death", "Death_Idle");

            _playerAudio.Play(_playerAudio.deathClip);
            _speed = 0f;

            isGameEnd = true;
            EventBus.OnRunEnded?.Invoke(_food, Mathf.FloorToInt(transform.position.x));
        }
        else
        {
            IFramesGlowing(2f);
        }

        Destroy(collision.gameObject);
        //Debug.LogError($"Collision - health: {_healthPoints}");
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

    private void PlayerDeath()
    {
        OnHealthChanged?.Invoke(_healthPoints, true);

        _speed = 0f;

        isGameEnd = true;
        EventBus.OnRunEnded?.Invoke(_food, Mathf.FloorToInt(transform.position.x));
        //Debug.LogError("Game Over");
    }

    private void PlayAnimation(string animation, string animation2)
    {
        Skeleton.AnimationState.SetAnimation(0, animation, false);
        Skeleton.AnimationState.AddAnimation(0, animation2, true, 0);
    }
}
