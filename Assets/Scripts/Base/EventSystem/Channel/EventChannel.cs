using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventChannel<T> : ScriptableObject
{
    [field:SerializeField] private readonly HashSet<EventListener<T>> observers = new HashSet<EventListener<T>>();

    public void Invoke(T value)
    {
        foreach (var observer in observers)
        {
            observer.Raise(value);
        }
    }

    public void Register(EventListener<T> observer) => observers.Add(observer);
    public void Deregister(EventListener<T> observer) => observers.Remove(observer);
}

public readonly struct Empty { }

[CreateAssetMenu(menuName = "Events/EventChannel")]
public class EventChannel : EventChannel<Empty> { }
