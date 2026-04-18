using FishRunner.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FishRunner.Systems
{
    public class SurroundingsGeneration : MonoBehaviour
    {
        private float _spawnPeriod = 18f;
        private IPlayerService _player = null;

        [SerializeField] private DateTime _spawnTime;

        private IMultiObjectPool _pool;

        
        void Start()
        {
            _player = ServiceLocator.Get<IPlayerService>();

            InstantiateObstacles();
            InstantiateFood();
        }

        public void Init(IMultiObjectPool pool)
        {
            _pool = pool;
        }

        void Update()
        {
            if (_player.IsGameEnd)
                return;

            if (Time.timeScale > 0f && (DateTime.Now - _spawnTime).TotalSeconds > _spawnPeriod / _player.Speed && UnityEngine.Random.Range(1, 100) > 50)
            {
                InstantiateObstacles();
                InstantiateFood();
            }
        }

        private void InstantiateFood()
        {
            Vector3 spawnPosition = new Vector3(_player.Position.x + UnityEngine.Random.Range(15, 17),
                    UnityEngine.Random.Range(-2, 3) * 2, 0);

            Instantiate(_pool.Get(ObstacleType.Food),
                spawnPosition, Quaternion.identity);

        }
        private void InstantiateObstacles()
        {
            Vector3 spawnPosition = Vector3.zero;
            switch (UnityEngine.Random.Range(0, 4))
            {
                case 0:
                    {
                        GameObject obstacle = null;
                        if (UnityEngine.Random.Range(0,2) == 0)
                        {
                            obstacle = _pool.Get(ObstacleType.Coral);
                            spawnPosition = new Vector3(
                                _player.Position.x + UnityEngine.Random.Range(17, 22),
                                UnityEngine.Random.Range(-3, -5), 0);
                        }
                        else
                        {
                            obstacle = _pool.Get(ObstacleType.NetAndTrash);
                            spawnPosition = new Vector3(_player.Position.x + UnityEngine.Random.Range(17, 22),
                                UnityEngine.Random.Range(-2, 3) * 2, _player.Position.z);
                        }
                        obstacle.transform.position = spawnPosition;
                        _spawnPeriod = 8f;
                        break;
                    }
                case 1:
                    {
                        spawnPosition = new Vector3(_player.Position.x + UnityEngine.Random.Range(17, 22),
                            -2, 0);
                        _pool.Get(ObstacleType.NetAndTrash).transform.position = spawnPosition;

                        spawnPosition = new Vector3(_player.Position.x + UnityEngine.Random.Range(17, 22),
                            2, 0);
                        _pool.Get(ObstacleType.NetAndTrash).transform.position = spawnPosition;
                        _spawnPeriod = 14f;
                        break;
                    }
                case 2:
                    {
                        spawnPosition = new Vector3(_player.Position.x + UnityEngine.Random.Range(17, 22),
                            -4, 0);
                        _pool.Get(ObstacleType.NetAndTrash).transform.position = spawnPosition;

                        spawnPosition = new Vector3(_player.Position.x + UnityEngine.Random.Range(17, 22),
                            0, 0);
                        _pool.Get(ObstacleType.NetAndTrash).transform.position = spawnPosition;

                        spawnPosition = new Vector3(_player.Position.x + UnityEngine.Random.Range(17, 22),
                            4, 0);
                        _pool.Get(ObstacleType.NetAndTrash).transform.position = spawnPosition;
                        _spawnPeriod = 18f;
                        break;
                    }
                case 3:
                    {
                        int minus = UnityEngine.Random.Range(0, 2) == 1 ? 1 : -1;
                        spawnPosition = new Vector3(_player.Position.x + 17f,
                            -4 * minus, 0);
                        _pool.Get(ObstacleType.NetAndTrash).transform.position = spawnPosition;
                        spawnPosition = new Vector3(_player.Position.x + 18.5f,
                            -2 * minus, 0);
                        _pool.Get(ObstacleType.NetAndTrash).transform.position = spawnPosition;
                        spawnPosition = new Vector3(_player.Position.x + 20f,
                            0, 0);
                        _pool.Get(ObstacleType.NetAndTrash).transform.position = spawnPosition;
                        spawnPosition = new Vector3(_player.Position.x + 21.5f,
                            2 * minus, 0);
                        _pool.Get(ObstacleType.NetAndTrash).transform.position = spawnPosition;
                        _spawnPeriod = 20f;
                        break;
                    }
            }
            _spawnTime = DateTime.Now;
        }
    }
}