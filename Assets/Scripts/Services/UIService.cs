using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishRunner.UI;
using TMPro;
using FishRunner.Configs;

namespace FishRunner.Services
{
    public class UIService : IUIService
    {
        private Transform _canvas;

        private UIConfig _config;

        public UIService(Transform canvas, UIConfig config)
        {
            _canvas = canvas;
            _config = config;
        }

        public void Instantiate(string id)
        {
            if (!_config.ObjectsDict.TryGetValue(id, out var prefab))
            {
                Debug.LogError($"UI prefab with id '{id}' not found.");
                return;
            }

            Object.Instantiate(prefab, _canvas);
        }
    }
}