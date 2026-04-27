using System;
using System.Collections.Generic;
using UnityEngine;

public class EventBus
{
    private static Dictionary<Type, Action<object>> events = new();

    /// <summary>
    /// Enregistre une action/Event contextuelle à l'input
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="callback"></param>
    public static void Subscribe<T>(Action<T> callback)
    {
        Type type = typeof(T);

        if (!events.ContainsKey(type))
            events[type] = delegate { };

        events[type] += (obj) => callback((T)obj);
        Debug.Log("S'abonne à :" + callback);
    }

    /// <summary>
    /// N'enregistre plus une action/Event
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="callback"></param>
    public static void Unsubscribe<T>(Action<T> callback)
    {
        Type type = typeof(T);

        if (events.ContainsKey(type))
            events[type] -= (obj) => callback((T)obj);

        Debug.Log("Se désabonne de :" + callback);
    }

    /// <summary>
    /// Publie un événement déclenché et envoi l'info
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    public static void Publish<T>(T data)
    {
        Type type = typeof(T);

        if (events.ContainsKey(type))
            events[type]?.Invoke(data);
        else
            Debug.LogWarning("No Event linked Found");
    }

}
