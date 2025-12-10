using System;
using System.Collections.Generic;

public static class EventBus
{
    private static Dictionary<Type, List<Delegate>> _subscribers = new();
    
    //구독
    public static void Subscribe<T>(Action<T> callback)
    {
        var type = typeof(T);
        if (!_subscribers.ContainsKey(type))
            _subscribers[type] = new List<Delegate>();

        _subscribers[type].Add(callback);
    }
    //해지
    public static void Unsubscribe<T>(Action<T> callback)
    {
        var type = typeof(T);
        if (_subscribers.ContainsKey(type))
            _subscribers[type].Remove(callback);
    }
    //발행
    public static void Publish<T>(T eventData)
    {
        var type = typeof(T);
        if (!_subscribers.ContainsKey(type))
            return;

        foreach (var del in _subscribers[type])
            (del as Action<T>)?.Invoke(eventData);
    }

}
