using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerParams", menuName = "Player/Player Params")]
public class PlayerParams : ScriptableObject
{
    [SerializeField] public int points { get; private set; } = 0;
    [SerializeField] public int healthPoints { get; private set; } = 3;
    [SerializeField] public float speed { get; private set; } = 4f;

    [SerializeField] public float acceleration { get; private set; } = 1.1f;
}
