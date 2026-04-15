using FishRunner.Configs;
using FishRunner.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FishRunner.Player
{
    public class PlayerInteractionHandler : MonoBehaviour
    {
        [SerializeField] public PlayerInteractionConfig config;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.TryGetComponent<PlayerControl>(out var player))
                return;

            player.ApplyInteraction(config);

            if (config.destroySource)
                Destroy(this.gameObject);
        }
    }
}