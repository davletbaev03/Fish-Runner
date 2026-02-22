using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerService
{
    public Vector3 Position { get; }
    public float Speed { get; }
    public bool IsGameEnd { get; }
}

public class PlayerService : IPlayerService
{
    private readonly PlayerControl _player;

    public PlayerService(PlayerControl player)
    {
        _player = player;
    }
    public Vector3 Position => _player.Position;
    public float Speed => _player.Speed;

    public bool IsGameEnd => _player.IsGameEnd;
}
