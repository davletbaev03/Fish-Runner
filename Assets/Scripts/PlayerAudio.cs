using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] public SoundConfig moveSideClip;
    [SerializeField] public SoundConfig deathClip;

    public void Play(SoundConfig sound)
    {
        if (sound == null || sound.clip == null)
            return;

        audioSource.pitch = sound.pitch;
        audioSource.volume = sound.volume;
        audioSource.PlayOneShot(sound.clip);
    }
}
