using FishRunner.Configs;
using FishRunner.Player;
using FishRunner.Services;
using FishRunner.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace FishRunner.Systems
{
    public class Bootstrap : MonoBehaviour
    {
        [Header ("UI")]
        [SerializeField] private Transform _canvas;
        [SerializeField] private UIConfig _configUI;

        [Header("Player")]
        [SerializeField] private PlayerControl _player;

        [Header("Player Audio")]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private SoundConfig _moveSideClip;
        [SerializeField] private SoundConfig _deathClip;

        [Header("Other")]
        [SerializeField] private SurroundingsGeneration _spawner = null;
        [SerializeField] private DespawnZone _despawnZone = null;

        [SerializeField] private List<GameObject> _coralPrefabs = null;
        [SerializeField] private List<GameObject> _foodPrefabs = null;
        [SerializeField] private List<GameObject> _netAndTrashPrefabs = null;

        private Dictionary<ObstacleType, List<GameObject>> _pool = new Dictionary<ObstacleType, List<GameObject>>();
        private Dictionary<ObstacleType, int> _poolCounts = new Dictionary<ObstacleType, int>
        {
            { ObstacleType.Coral, 8 },
            { ObstacleType.Food, 5 },
            { ObstacleType.NetAndTrash, 15 }
        };

        private void Awake()
        {
            var UIService = new UIService(_canvas, _configUI);
            ServiceLocator.Register<IUIService>(UIService);

            var playerService = new PlayerService(_player);
            ServiceLocator.Register<IPlayerService>(playerService);

            var playerAudioService = new PlayerAudioService(_audioSource, _moveSideClip, _deathClip);
            ServiceLocator.Register<IPlayerAudioService>(playerAudioService);

            if (!ServiceLocator.Services.ContainsKey(typeof(ISaveLoadService)))
            {
                var saveLoadService = new SaveLoadService();
                ServiceLocator.Register<ISaveLoadService>(saveLoadService);

                var analyticService = new AnalyticService();
                ServiceLocator.Register<IAnalyticService>(analyticService);

                var recordsManager = new RecordsManager();
                ServiceLocator.Register<IRecordsManager>(recordsManager);
            }

            _pool[ObstacleType.Coral] = _coralPrefabs;
            _pool[ObstacleType.Food] = _foodPrefabs;
            _pool[ObstacleType.NetAndTrash] = _netAndTrashPrefabs;

            ObjectsFactory factory = new UnityObjectFactory();
            IMultiObjectPool pool = new MultiObjectPool(_pool, _poolCounts, factory);
            _spawner.Init(pool);
            _despawnZone.Init(pool);
        }
    }
}
