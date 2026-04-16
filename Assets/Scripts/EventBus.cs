using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

namespace FishRunner.Systems
{
    public static class EventBus
    {
        

        private static readonly Dictionary<Type, Delegate> Events = new();

        public static void Subscribe<T>(Action<T> callback)
        {
            var type = typeof(T);

            //Debug.Log($"[EVENT BUS] Subscribe {callback.Method.DeclaringType?.Name}.{callback.Method.Name} -> {type.Name}");

            if (Events.TryGetValue(type, out var existing))
                Events[type] = (Action<T>)existing + callback;
            else
                Events[type] = callback;
        }

        public static void Unsubscribe<T>(Action<T> callback)
        {
            var type = typeof(T);

            //Debug.Log($"[EVENT BUS] Unsubscribe {callback.Method.DeclaringType?.Name}.{callback.Method.Name} -> {type.Name}");

            if (!Events.TryGetValue(type, out var existing))
                return;

            var result = (Action<T>)existing - callback;

            if (result == null)
                Events.Remove(type);
            else
                Events[type] = result;
        }

        public static void RaiseEvent<T>(T eventData)
        {
            var type = typeof(T);

            //Debug.Log($"[EVENT BUS] Invoke {type.Name}");

            if (!Events.TryGetValue(type, out var existing))
            {
                //Debug.LogWarning($"[EVENT BUS] No listeners for {type.Name}");
                return;
            }

            foreach (var listener in existing.GetInvocationList())
            {
                //Debug.Log($"[EVENT BUS] -> {listener.Method.DeclaringType?.Name}.{listener.Method.Name}");
            }

            ((Action<T>)existing)?.Invoke(eventData);
        }
    }
}