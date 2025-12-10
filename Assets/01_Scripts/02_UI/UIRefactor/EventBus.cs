using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventBus
{
    private static readonly Dictionary<Type, IList> _subscribers = new();

    private static List<WeakReference<Action<T>>> GetList<T>()
    {
        var type = typeof(T);

        if (!_subscribers.TryGetValue(type, out var list))
        {
            list = new List<WeakReference<Action<T>>>();
            _subscribers[type] = list;
        }

        return (List<WeakReference<Action<T>>>)list;
    }

    public static void Subscribe<T>(Action<T> callback)
    {
        GetList<T>().Add(new WeakReference<Action<T>>(callback));
    }

    public static void Unsubscribe<T>(Action<T> callback)
    {
        var list = GetList<T>();

        list.RemoveAll(w =>
        {
            if (!w.TryGetTarget(out var target))
                return true;

            return target == callback;
        });
    }

    public static void Publish<T>(T eventData)
    {
        var list = GetList<T>();

        var snapshot = list.ToArray();

        foreach (var weak in snapshot)
        {
            if (!weak.TryGetTarget(out var callback))
            {
                list.Remove(weak);
                continue;
            }

            try
            {
                callback.Invoke(eventData);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[EventBus] {typeof(T).Name} 처리 중 예외: {ex}");
            }
        }
    }
}

