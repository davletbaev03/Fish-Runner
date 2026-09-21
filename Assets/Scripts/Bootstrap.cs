using FishRunner.Configs;
using FishRunner.Player;
using FishRunner.Services;
using FishRunner.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace FishRunner.Systems
{
    public class Bootstrap : MonoBehaviour
    {
        [Header("Player Audio")]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private SoundConfig _moveSideClip;
        [SerializeField] private SoundConfig _deathClip;

        [Header("Other")]
        [SerializeField] private SurroundingsGeneration _spawner = null;
        [SerializeField] private DespawnZone _despawnZone = null;

        [Header("Prefubs")]
        [SerializeField] private List<GameObject> _prefabs = null;
        [SerializeField] private List<Sprite> _coralSprites = null;
        [SerializeField] private List<Sprite> _foodSprites = null;
        [SerializeField] private List<Sprite> _netSprites = null;
        [SerializeField] private List<Sprite> _trashSprites = null;

        private Dictionary<ObstacleType, GameObject> _pool = new Dictionary<ObstacleType, GameObject>();
        private Dictionary<ObstacleType, int> _poolCounts = new Dictionary<ObstacleType, int>
        {
            { ObstacleType.Coral, 8 },
            { ObstacleType.Food, 5 },
            { ObstacleType.Net, 15 },
            { ObstacleType.Trash, 8 }
        };

        private void Awake()
        {
            var playerAudioService = new PlayerAudioService(_audioSource, _moveSideClip, _deathClip);
            ServiceLocator.Register<IPlayerAudioService>(playerAudioService);

            if (!ServiceLocator.Services.ContainsKey(typeof(ISaveLoadService)))
            {
                var saveLoadService = new SaveLoadService();
                ServiceLocator.Register<ISaveLoadService>(saveLoadService);

            }

            _pool[ObstacleType.Coral] = _prefabs[0];
            _pool[ObstacleType.Food] = _prefabs[1];
            _pool[ObstacleType.Net] = _prefabs[2];
            _pool[ObstacleType.Trash] = _prefabs[3];

            var factoryDictionary = new Dictionary<ObstacleType, List<Sprite>>
{
                { ObstacleType.Coral, _coralSprites },
                { ObstacleType.Food, _foodSprites },
                { ObstacleType.Net, _netSprites },
                { ObstacleType.Trash, _trashSprites }
            };

            ObjectsFactory factory = new UnityObjectFactory(factoryDictionary);

            IMultiObjectPool pool = new MultiObjectPool(_pool, _poolCounts, factory);
            _spawner.Init(pool);
            _despawnZone.Init(pool);
        }
    }
}
