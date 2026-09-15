using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FishRunner.Configs
{

    [CreateAssetMenu(fileName = nameof(UIConfig), menuName = "UI/Config")]
    public class UIConfig : ScriptableObject
    {
        [Serializable]
        private struct UIObject
        {
            public string Id;
            public GameObject Object; 
        }

        [SerializeField] private List<UIObject> _objects = new List<UIObject>();

        private Dictionary<string, GameObject> _objectsDict = null;
        public Dictionary<string, GameObject> ObjectsDict 
        { 
            get 
            {
                if (_objectsDict == null)
                {
                    _objectsDict = new Dictionary<string, GameObject>();
                    foreach (var obj in _objects)
                    {
                        _objectsDict[obj.Id] =  obj.Object;
                    }
                }
                return _objectsDict; 
            } 
        }
    }
}