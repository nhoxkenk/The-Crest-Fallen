using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    private static readonly Dictionary<Type, Action<GameEvent>> events = new Dictionary<Type, Action<GameEvent>>();
    private static readonly Dictionary<Delegate, Action<GameEvent>> eventLookups = new Dictionary<Delegate, Action<GameEvent>>();

    public static void Register<T>(Action<T> @event) where T : GameEvent
    {
        if (!eventLookups.ContainsKey(@event))
        {
            //Create new

        }
    }

    public static void Unregister()
    {

    }

    public static void Raise(GameEvent @event)
    {
        if(events.TryGetValue(@event.GetType(), out var action))
        {
            action?.Invoke(@event);
        }
    }

    public static void Remove()
    {
        events.Clear();
        eventLookups.Clear();
    }
}
