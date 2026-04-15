using FishRunner.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuBootstrap : MonoBehaviour
{
    private void Awake()
    {
        var analyticService = new AnalyticService();
        ServiceLocator.Register<IAnalyticService>(analyticService);

        var saveLoadService = new SaveLoadService();
        ServiceLocator.Register<ISaveLoadService>(saveLoadService);

        var recordsManager = new RecordsManager();
        ServiceLocator.Register<IRecordsManager>(recordsManager);
    }
}
