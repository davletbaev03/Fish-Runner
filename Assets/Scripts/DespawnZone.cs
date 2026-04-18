using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FishRunner.Systems
{
    public class DespawnZone : MonoBehaviour
    {
        IMultiObjectPool _pool = null;

        public void Init(IMultiObjectPool pool)
        {
            _pool = pool;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Obstacle") && !other.CompareTag("Food") && !other.CompareTag("Coral"))
                return;

            if(other.CompareTag("Coral"))
                _pool.Release(other.gameObject, ObstacleType.Coral);
            else if (other.CompareTag("Food"))
                _pool.Release(other.gameObject, ObstacleType.Food);
            else
                _pool.Release(other.gameObject, ObstacleType.NetAndTrash);
        }
    }
}