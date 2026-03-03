using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public interface IPlayerAudioService
{
    public AudioSource AudioSource { get; }
    public SoundConfig MoveSideClip { get; }
    public SoundConfig DeathClip { get; }

    public void Play(SoundConfig sound);

}

public class PlayerAudioService : IPlayerAudioService
{
    private AudioSource audioSource;
    private SoundConfig moveSideClip;
    private SoundConfig deathClip;

    public PlayerAudioService(AudioSource audioSource, SoundConfig moveSideClip, SoundConfig deathClip)
    {
        this.audioSource = audioSource;
        this.moveSideClip = moveSideClip;
        this.deathClip = deathClip;
    }

    public AudioSource AudioSource => audioSource;
    public SoundConfig MoveSideClip => moveSideClip;
    public SoundConfig DeathClip => deathClip;

    public void Play(SoundConfig sound)
    {
        if (sound == null || sound.clip == null)
            return;

        audioSource.pitch = sound.pitch;
        audioSource.volume = sound.volume;
        audioSource.PlayOneShot(sound.clip);
    }
}
