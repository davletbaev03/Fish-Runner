using FishRunner.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace FishRunner.Systems
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindServices();
            DontDestroyOnLoad(this);
        }

        private void BindServices()
        {
            Container.Bind<ISaveLoadService>()
                .To<SaveLoadService>()
                .AsSingle();

            Container.Bind<IRecordsManager>()
                .To<RecordsManager>()
                .AsSingle();

            Container.Bind<IAnalyticService>()
                .To<AnalyticService>()
                .AsSingle();
        }
    }
}