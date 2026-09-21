using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FishRunner.UI;
using System;

namespace FishRunner.Services
{
    public class LoadingService : ILoadingService
    {
        private readonly WindowLoadingScreen _loadingScreen;

        public LoadingService(WindowLoadingScreen loadingScreen)
        {
            _loadingScreen = loadingScreen;
        }
        public async UniTask LoadScene(string sceneName)
        {
            _loadingScreen.Show();

            await SceneManager.LoadSceneAsync(sceneName);

            _loadingScreen.Hide();
        }
    }
}