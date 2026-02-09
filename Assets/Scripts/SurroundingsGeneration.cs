using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurroundingsGeneration : MonoBehaviour
{
    [SerializeField] private PlayerControl _player = null;

    [SerializeField] private List<GameObject> _obstacles = new List<GameObject>();
    [SerializeField] private List<GameObject> _food = new List<GameObject>();

    [SerializeField] private DateTime _spawnTime;

    void Start()
    {
        InstantiateObstacles();
        InstantiateFood();
    }

    

    void Update()
    {
        if (_player.isGameEnd)
            return;

        if(Time.timeScale > 0f && (DateTime.Now - _spawnTime).TotalSeconds > 18f/_player.Speed && UnityEngine.Random.Range(1, 100) > 50)
        {
            InstantiateObstacles();
            InstantiateFood();
        }
    }

    private void InstantiateFood()
    {
        int foodNumber = UnityEngine.Random.Range(0, _food.Count);
        Vector3 spawnPosition = new Vector3(_player.transform.position.x + UnityEngine.Random.Range(15, 17),
                UnityEngine.Random.Range(-2, 3) * 2, 0);

        Instantiate(_food[foodNumber], spawnPosition, Quaternion.identity);

    }
    private void InstantiateObstacles()
    {
        int obstacleNumber = UnityEngine.Random.Range(0, _obstacles.Count);
        Vector3 spawnPosition = Vector3.zero;
        switch (UnityEngine.Random.Range(0, 4))
        {
            case 0:
                {
                    if (_obstacles[obstacleNumber].name.StartsWith("coral"))
                    {
                        spawnPosition = new Vector3(
                            _player.transform.position.x + UnityEngine.Random.Range(17, 22),
                            UnityEngine.Random.Range(-3, -5), 0);
                    }
                    else
                    {
                        spawnPosition = new Vector3(_player.transform.position.x + UnityEngine.Random.Range(17, 22),
                            UnityEngine.Random.Range(-2, 3) * 2, _player.transform.position.z);
                    }
                    Instantiate(_obstacles[obstacleNumber], spawnPosition, Quaternion.identity);
                    break;
                }
            case 1:
                {
                    spawnPosition = new Vector3(_player.transform.position.x + UnityEngine.Random.Range(17, 22),
                        -2, 0);
                    Instantiate(_obstacles[UnityEngine.Random.Range(17, 27)], spawnPosition, Quaternion.identity);

                    spawnPosition = new Vector3(_player.transform.position.x + UnityEngine.Random.Range(17, 22),
                        2, 0);
                    Instantiate(_obstacles[UnityEngine.Random.Range(17, 27)], spawnPosition, Quaternion.identity);
                    break;
                }
            case 2:
                {
                    spawnPosition = new Vector3(_player.transform.position.x + UnityEngine.Random.Range(17, 22),
                        -4, 0);
                    Instantiate(_obstacles[UnityEngine.Random.Range(17, 27)], spawnPosition, Quaternion.identity);

                    spawnPosition = new Vector3(_player.transform.position.x + UnityEngine.Random.Range(17, 22),
                        0, 0);
                    Instantiate(_obstacles[UnityEngine.Random.Range(17, 27)], spawnPosition, Quaternion.identity);

                    spawnPosition = new Vector3(_player.transform.position.x + UnityEngine.Random.Range(17, 22),
                        4, 0);
                    Instantiate(_obstacles[UnityEngine.Random.Range(17, 27)], spawnPosition, Quaternion.identity);
                    break;
                }
            case 3:
                {
                    int minus = UnityEngine.Random.Range(0, 2) == 1 ? 1 : -1;
                    spawnPosition = new Vector3(_player.transform.position.x + 17f,
                        -4 * minus, 0);
                    Instantiate(_obstacles[UnityEngine.Random.Range(17, 27)], spawnPosition, Quaternion.identity);
                    spawnPosition = new Vector3(_player.transform.position.x + 18.5f,
                        -2 * minus, 0);
                    Instantiate(_obstacles[UnityEngine.Random.Range(17, 27)], spawnPosition, Quaternion.identity);
                    spawnPosition = new Vector3(_player.transform.position.x + 20f,
                        0, 0);
                    Instantiate(_obstacles[UnityEngine.Random.Range(17, 27)], spawnPosition, Quaternion.identity);
                    spawnPosition = new Vector3(_player.transform.position.x + 21.5f,
                        2 * minus, 0);
                    Instantiate(_obstacles[UnityEngine.Random.Range(17, 27)], spawnPosition, Quaternion.identity);
                    break;
                }
         }
                _spawnTime = DateTime.Now;
    }
}
