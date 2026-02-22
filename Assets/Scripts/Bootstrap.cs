using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private PlayerControl _player;

    private void Awake()
    {
        var playerService = new PlayerService(_player);
        ServiceLocator.Register<IPlayerService>(playerService);
    }
}
