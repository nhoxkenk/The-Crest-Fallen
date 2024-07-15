using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    private static readonly Dictionary<Type, Action<GameEvent>> events = new Dictionary<Type, Action<GameEvent>>();
    private static readonly Dictionary<Delegate, Action<GameEvent>> eventLookups = new Dictionary<Delegate, Action<GameEvent>>();

    public static void RegisterEvent<T>(Action<T> @event) where T : GameEvent
    {
        if (!eventLookups.ContainsKey(@event))
        {
            //Create new
            Action<GameEvent> newAction = (e) => @event((T) e);
            eventLookups[@event] = newAction;   

            if(events.TryGetValue(typeof(T), out Action<GameEvent> internalAction))
            {
                events[typeof(T)] = internalAction += newAction;
            }
            else
            {
                events[typeof(T)] = newAction;
            }
        }
    }

    public static void UnregisterEvent<T>(Action<T> @event) where T : GameEvent
    {
        if (eventLookups.TryGetValue(@event, out var action))
        {
            if (events.TryGetValue(typeof(T), out var tempAction))
            {
                tempAction -= action;
                if (tempAction == null)
                    events.Remove(typeof(T));
                else
                    events[typeof(T)] = tempAction;
            }

            eventLookups.Remove(@event);
        }
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
