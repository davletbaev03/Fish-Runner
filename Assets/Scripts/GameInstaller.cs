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

        [Header("Other")]
        [SerializeField] private GameController _gameController;


        public override void InstallBindings()
        {
            Debug.Log("INSTALLER");
            Container.Bind<PlayerControl>()
                .FromInstance(_player);

            Container.Bind<IPlayerService>()
                .To<PlayerService>()
                .AsSingle();

            Container.Bind<Transform>()
                .FromInstance(_canvas);

            Container.Bind<UIConfig>()
                .FromInstance(_configUI);

            Container.Bind<IUIService>()
                .To<UIService>()
                .AsSingle();

            Container.Bind<IAnalyticService>()
                .To<AnalyticService>()
                .AsSingle();

            Container.Bind<IRecordsManager>()
                .To<RecordsManager>()
                .AsSingle();

            Container.Bind<ISaveLoadService>()
                .To<SaveLoadService>()
                .AsSingle();

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
    }
}