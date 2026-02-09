using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/Interaction Config")]
public class PlayerInteractionConfig : ScriptableObject
{
    [Header("Points")]
    public int addPoints = 0;

    [Header("Health")]
    public int damage = 1;
    public bool killInstant = false;

    [Header("Animation")]
    public string animation = "Damage";
    public string animation2 = "Swim_Normal";

    [Header("Audio")]
    public SoundConfig sound;

    [Header("Destroy")]
    public bool destroySource = true;
}
