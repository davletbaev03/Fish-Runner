using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip moveSideClip;
    [SerializeField] private AudioClip eatClip;
    [SerializeField] private AudioClip deathClip;
    [SerializeField] private AudioClip hitClip;

    public void PlayMoveSide() => audioSource.PlayOneShot(moveSideClip);
    public void PlayEat() => audioSource.PlayOneShot(eatClip);
    public void PlayDeath() => audioSource.PlayOneShot(deathClip);
    public void PlayHit() => audioSource.PlayOneShot(hitClip);
}
