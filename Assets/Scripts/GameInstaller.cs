using FishRunner.Configs;
using FishRunner.Player;
using FishRunner.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace FishRunner.Systems
{
    public class GameInstaller : MonoInstaller
    {
        [Header("UI")]
        [SerializeField] private Transform _canvas;
        [SerializeField] private UIConfig _configUI;

        [Header("Player")]
        [SerializeField] private PlayerControl _player;

        [Header("Player Audio")]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private SoundConfig _moveSideClip;
        [SerializeField] private SoundConfig _deathClip;

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

        [Header("Systems")]
        [SerializeField] private SurroundingsGeneration _spawner = null;
        [SerializeField] private DespawnZone _despawnZone = null;
        [SerializeField] private GameController _gameController;


        public override void InstallBindings()
        {
            BindPlayer();
            BindUI();
            BindAudio();
            BindPool();
        }

        private void BindPlayer()
        {
            Container.Bind<PlayerControl>()
                .FromInstance(_player);

            Container.Bind<IPlayerService>()
                .To<PlayerService>()
                .AsSingle();
        }

        private void BindUI()
        {
            Container.Bind<Transform>()
                .FromInstance(_canvas);

            Container.Bind<UIConfig>()
                .FromInstance(_configUI);

            Container.Bind<IUIService>()
                .To<UIService>()
                .AsSingle();
        }

        private void BindAudio()
        {
            Container.Bind<AudioSource>()
                .FromInstance(_audioSource);

            Container.Bind<SoundConfig>()
                .WithId("Move")
                .FromInstance(_moveSideClip);

            Container.Bind<SoundConfig>()
                .WithId("Death")
                .FromInstance(_deathClip);

            Container.Bind<IPlayerAudioService>()
                .To<PlayerAudioService>()
                .AsSingle();
        }

        private void BindPool()
        {
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

            Container.Bind<IMultiObjectPool>()
                .FromInstance(pool)
                .AsSingle();

            Container.Bind<SurroundingsGeneration>()
                .FromInstance(_spawner)
                .NonLazy();

            Container.Bind<DespawnZone>()
                .FromInstance(_despawnZone)
                .NonLazy();
        }
    }
}