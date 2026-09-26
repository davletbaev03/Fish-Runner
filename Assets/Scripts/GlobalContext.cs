using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FishRunner.Systems
{
    public class GlobalContext : MonoBehaviour
    {
        private static GlobalContext _instance;

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}