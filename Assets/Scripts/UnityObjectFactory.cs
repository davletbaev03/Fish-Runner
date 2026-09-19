using FishRunner.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityObjectFactory : ObjectsFactory
{
    private Dictionary<ObstacleType, List<Sprite>> _sprites;

    public UnityObjectFactory(Dictionary<ObstacleType, List<Sprite>> sprites)
    {
        _sprites = sprites;
    }

    public override GameObject Create(GameObject prefab, ObstacleType type)
    {
        GameObject obj = Object.Instantiate(prefab);
        obj.GetComponent<SpriteRenderer>().sprite = 
            _sprites[type][Random.Range(0, _sprites[type].Count - 1)];
        return obj;
    }
}
