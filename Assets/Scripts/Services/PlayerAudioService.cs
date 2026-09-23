using FishRunner.Configs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using static Unity.VisualScripting.Member;

namespace FishRunner.Services
{
    public class PlayerAudioService : IPlayerAudioService
    {
        private AudioSource _audioSource;
        private SoundConfig _moveSideClip;
        private SoundConfig _deathClip;

        public PlayerAudioService(AudioSource audioSource,
            [Inject(Id = "Move")] SoundConfig moveSideClip,
            [Inject(Id = "Death")] SoundConfig deathClip)
        {
            _audioSource = audioSource;
            _moveSideClip = moveSideClip;
            _deathClip = deathClip;
        }

        public AudioSource AudioSource => _audioSource;
        public SoundConfig MoveSideClip => _moveSideClip;
        public SoundConfig DeathClip => _deathClip;

        public void Play(SoundConfig sound)
        {
            if (sound == null || sound.clip == null)
                return;

            _audioSource.pitch = sound.pitch;
            _audioSource.volume = sound.volume;
            _audioSource.PlayOneShot(sound.clip);
        }
    }
}