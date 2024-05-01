using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class EventListener<T> : MonoBehaviour
{
    [SerializeField] private EventChannel<T> eventChannel;
    [SerializeField] private UnityEvent<T> unityEvent;

    private void Awake()
    {
        eventChannel.Register(this);    
    }

    private void OnDestroy()
    {
        eventChannel.Deregister(this);
    }

    public void Raise(T value)
    {
        unityEvent?.Invoke(value);
    }
}

public class EventListener : EventListener<Empty> { }
