using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventBus
{
    //구독자를 타입형태로 리스트로 관리
    private static readonly Dictionary<Type, IList> _subscribers = new();

    //구독자가 실행할 리스트 불러오기
    private static List<WeakReference<Action<T>>> GetList<T>()
    {
        var type = typeof(T);

        if (!_subscribers.TryGetValue(type, out var list))
        {
            list = new List<WeakReference<Action<T>>>(); // 리스트가 없다면 새로 생성
            _subscribers[type] = list;
        }

        return (List<WeakReference<Action<T>>>)list; //리스트가 있다면 유지
    }

    //구독
    public static void Subscribe<T>(Action<T> callback)
    {
        GetList<T>().Add(new WeakReference<Action<T>>(callback));
    }

    //해지
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

    //발행
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

            // 예외처리
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

