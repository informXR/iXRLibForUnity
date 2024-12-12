using System;
using System.Collections.Generic;
using UnityEngine;

public static class ServiceLocator
{
    private static readonly Dictionary<Type, object> Services = new();

    public static void RegisterService<T>(T service)
    {
        var type = typeof(T);
        if (Services.ContainsKey(type))
        {
            Debug.LogWarning($"Service of type {type.Name} is already registered.");
            return;
        }
        
        Services[type] = service;
    }

    public static T GetService<T>()
    {
        var type = typeof(T);
        if (!Services.ContainsKey(type))
        {
            throw new Exception($"Service of type {type.Name} is not registered.");
        }

        return (T)Services[type];
    }

    public static void Clear()
    {
        Services.Clear();
    }
}