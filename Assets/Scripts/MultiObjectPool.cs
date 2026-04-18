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
        private Dictionary<ObstacleType, List<GameObject>> _prefabs;

        private Dictionary<ObstacleType, Queue<GameObject>> _pools = new();

        public MultiObjectPool(Dictionary<ObstacleType, List<GameObject>> prefabs, Dictionary<ObstacleType, int> counts)
        {
            _prefabs = prefabs;

            foreach (var pair in _prefabs)
            {
                InitializeQueue(pair.Value, counts[pair.Key], pair.Key);
            }
        }

        private void InitializeQueue(List<GameObject> list, int count, ObstacleType type)
        {
            _pools[type] = new Queue<GameObject>();

            for (int i = 0; i < count; i++)
            {
                var obj = Object.Instantiate(list[Random.Range(0, list.Count)], new Vector3 (1f, 10f, 1f), Quaternion.identity);
                obj.gameObject.SetActive(false);
                _pools[type].Enqueue(obj);
            }
        }

        public GameObject Get(ObstacleType type)
        {
            if (_pools[type].Count == 0)
            {
                var obj = Object.Instantiate(_prefabs[type][Random.Range(0, _prefabs[type].Count)]);
                return obj;
            }

            var item = _pools[type].Dequeue();
            item.gameObject.SetActive(true);
            return item;
        }

        public void Release(GameObject item, ObstacleType type)
        {
            item.gameObject.SetActive(false);
            _pools[type].Enqueue(item);
        }
    }
}