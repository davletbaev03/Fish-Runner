using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    //player
    [SerializeField] private PlayerControl _player;

    //playerAudio
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private SoundConfig _moveSideClip;
    [SerializeField] private SoundConfig _deathClip;

    private void Awake()
    {
        var playerService = new PlayerService(_player);
        ServiceLocator.Register<IPlayerService>(playerService);

        var analyticService = new AnalyticService();
        ServiceLocator.Register<IAnalyticService>(analyticService);

        var PlayerAudioService = new PlayerAudioService(_audioSource, _moveSideClip, _deathClip);
        ServiceLocator.Register<IPlayerAudioService>(PlayerAudioService);

        var SaveLoadService = new SaveLoadService();
        ServiceLocator.Register<ISaveLoadService>(SaveLoadService);
    }
}
