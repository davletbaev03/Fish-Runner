using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace FishRunner.Systems
{
    public class DespawnZone : MonoBehaviour
    {
        IMultiObjectPool _pool = null;

        [Inject]
        public void Init(IMultiObjectPool pool)
        {
            _pool = pool;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent<PoolObject>(out var poolObject))
                return;


            if (other.CompareTag("Coral"))
                _pool.Release(other.gameObject, ObstacleType.Coral);
            else if (other.CompareTag("Food"))
                _pool.Release(other.gameObject, ObstacleType.Food);
            else if (other.CompareTag("Trash"))
                _pool.Release(other.gameObject, ObstacleType.Trash);
            else
                _pool.Release(other.gameObject, ObstacleType.Net);
        }
    }
}