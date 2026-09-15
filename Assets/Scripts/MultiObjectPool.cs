using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FishRunner.Systems
{
    public enum ObstacleType
    {
        Food,
        NetAndTrash,
        Coral
    }
    public interface IMultiObjectPool
    {
        public GameObject Get(ObstacleType type);
        public void Release(GameObject item, ObstacleType type);

    }

    public class MultiObjectPool : IMultiObjectPool
    {
        private Dictionary<ObstacleType, List<GameObject>> _prefabsConfig;

        private Dictionary<ObstacleType, Queue<GameObject>> _pools = new();

        public MultiObjectPool(Dictionary<ObstacleType, List<GameObject>> prefabs, 
            Dictionary<ObstacleType, int> counts)
        {
            _prefabsConfig = prefabs;

            foreach (var pair in _prefabsConfig)
            {
                InitializeHeatQueue(pair.Value, counts[pair.Key], pair.Key);
            }
        }

        private void InitializeHeatQueue(List<GameObject> list, int count, ObstacleType type)
        {
            _pools[type] = new Queue<GameObject>();

            for (int i = 0; i < count; i++)
            {
                var obj = Object.Instantiate(list[Random.Range(0, list.Count)], 
                    new Vector3 (1f, 10f, 1f), Quaternion.identity);
                obj.gameObject.SetActive(false);
                _pools[type].Enqueue(obj);
            }
        }

        public GameObject Get(ObstacleType type)
        {
            if (_pools[type].Count > 0)
            {
                var item = _pools[type].Dequeue();
                item.gameObject.SetActive(true);
                return item;
            }

            var obj = Object.Instantiate(_prefabsConfig[type][Random.Range(0, _prefabsConfig[type].Count)]);
            return obj;

        }

        public void Release(GameObject item, ObstacleType type)
        {
            item.gameObject.SetActive(false);
            _pools[type].Enqueue(item);
        }
    }
}