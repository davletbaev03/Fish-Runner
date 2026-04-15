using FishRunner.Configs;
using UnityEngine;

namespace FishRunner.Services
{
    public interface IPlayerAudioService
    {
        public AudioSource AudioSource { get; }
        public SoundConfig MoveSideClip { get; }
        public SoundConfig DeathClip { get; }

        public void Play(SoundConfig sound);

    }
}