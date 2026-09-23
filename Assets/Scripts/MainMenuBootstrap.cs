using FishRunner.Services;
using FishRunner.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuBootstrap : MonoBehaviour
{
    [SerializeField] private WindowLoadingScreen _windowLoadingScreen = null;

    private void Awake()
    {
        var loadingService = new LoadingService(_windowLoadingScreen);
        ServiceLocator.Register<ILoadingService>(loadingService);

        var analyticService = new AnalyticService();
        ServiceLocator.Register<IAnalyticService>(analyticService);

        var saveLoadService = new SaveLoadService();
        ServiceLocator.Register<ISaveLoadService>(saveLoadService);
    }
}
