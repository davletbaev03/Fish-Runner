using System;
using System.Collections.Generic;
using UnityEngine;

public static class ServiceLocator
{
    private static Dictionary<Type, object> _services = new();

    public static void Register<T>(T service)
    {
        var type = typeof(T);

        if (_services.ContainsKey(type))
        {
            Debug.LogError($"Service {type.Name} already registered");
        }

        _services[type] = service;
    }

    public static T Get<T>()
    {
        var type = typeof(T);

        if (_services.TryGetValue(type, out var service))
        {
            return (T)service;
        }

        throw new Exception($"Service {type.Name} not found");
    }

    public static void Clear()
    {
        _services.Clear();
    }
}
