using FishRunner.Configs;
using FishRunner.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

namespace FishRunner.Services
{
    public class UIService : IUIService
    {
        private Transform _canvas;

        private UIConfig _config;

        private readonly DiContainer _container;

        public UIService(Transform canvas, UIConfig config, DiContainer container)
        {
            _canvas = canvas;
            _config = config;
            _container = container;
        }

        public void Instantiate(string id)
        {
            if (!_config.ObjectsDict.TryGetValue(id, out var prefab))
            {
                Debug.LogError($"UI prefab with id '{id}' not found.");
                return;
            }

            _container.InstantiatePrefab(prefab, _canvas);
        }
    }
}