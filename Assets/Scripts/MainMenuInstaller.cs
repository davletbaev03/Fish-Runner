using FishRunner.Services;
using FishRunner.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MainMenuInstaller : MonoInstaller
{
    [SerializeField] private WindowLoadingScreen _windowLoadingScreen = null;

    public override void InstallBindings()
    {
        BindLoadingScreen();

    }

    private void BindLoadingScreen()
    {
        Container.Bind<ILoadingService>()
            .To<LoadingService>()
            .AsSingle();
        Container.Bind<WindowLoadingScreen>()
            .FromInstance(_windowLoadingScreen)
            .AsSingle();
    }
}
