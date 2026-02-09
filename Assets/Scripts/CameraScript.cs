using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraScript : MonoBehaviour
{
    [SerializeField] private PlayerControl _player;
    [SerializeField] private AudioSource _source;
    void Start()
    {
        if(_player == null)
            this.transform.position = new Vector3(_player.transform.position.x + 5, transform.position.y, transform.position.z);
        _player.OnGameEnd += StopMusic;
    }
    void Update()
    {
        this.transform.position = new Vector3(_player.transform.position.x + 5, transform.position.y, transform.position.z);
    }

    private void StopMusic(int a, int b)
    {
        _source.Stop();
    }

    private void OnDestroy()
    {
        _player.OnGameEnd -= StopMusic;
    }
}
