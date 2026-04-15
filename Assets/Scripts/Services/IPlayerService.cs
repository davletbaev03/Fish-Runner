using UnityEngine;

namespace FishRunner.Services
{
    public interface IPlayerService
    {
        public Vector3 Position { get; }
        public float Speed { get; }
        public bool IsGameEnd { get; }
    }
}