using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FishRunner.Systems
{
    public enum ObstacleType
    {
        Food,
        Net,
        Coral,
        Trash
    }
    public interface IMultiObjectPool
    {
        public GameObject Get(ObstacleType type);
        public void Release(GameObject item, ObstacleType type);

    }

    public class MultiObjectPool : IMultiObjectPool
    {
        private ObjectsFactory _factory;

        private Dictionary<ObstacleType, GameObject> _prefabsConfig;

        private Dictionary<ObstacleType, Queue<GameObject>> _pools = new();

        public MultiObjectPool(Dictionary<ObstacleType, GameObject> prefabs, 
            Dictionary<ObstacleType, int> counts,
            ObjectsFactory factory)
        {
            _prefabsConfig = prefabs;
            _factory = factory;

            foreach (var pair in _prefabsConfig)
            {
                InitializeHeatQueue(pair.Value, counts[pair.Key], pair.Key);
            }
        }

        private void InitializeHeatQueue(GameObject prefab, int count, ObstacleType type)
        {
            _pools[type] = new Queue<GameObject>();

            for (int i = 0; i < count; i++)
            {
                var obj = _factory.Create(prefab,type);

                obj.transform.position = new Vector3(1f, 10f, 1f);
                obj.gameObject.SetActive(false);
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

            var obj = _factory.Create(_prefabsConfig[type], type);

            return obj;
        }

        public void Release(GameObject item, ObstacleType type)
        {
            item.gameObject.SetActive(false);
            _pools[type].Enqueue(item);
        }
    }
}