using DG.Tweening;
using FishRunner.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FishRunner.Systems
{
    public class CameraScript : MonoBehaviour
    {
        private IPlayerService _player;
        [SerializeField] private AudioSource _source;
        void Start()
        {
            _player = ServiceLocator.Get<IPlayerService>();
            if (_player == null)
                this.transform.position = new Vector3(_player.Position.x + 5, transform.position.y, transform.position.z);
            Systems.EventBus.OnRunEnded += StopMusic;
        }
        void Update()
        {
            this.transform.position = new Vector3(_player.Position.x + 5, transform.position.y, transform.position.z);
        }

        private void StopMusic(int a, int b)
        {
            _source.Stop();
        }

        private void OnDestroy()
        {
            Systems.EventBus.OnRunEnded -= StopMusic;
        }
    }
}