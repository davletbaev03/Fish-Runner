using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityObjectFactory : ObjectsFactory
{
    public override GameObject Create(GameObject prefab)
    {
        return Object.Instantiate(prefab);
    }
}
