using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Events
{
    public static CharacterHealthChangedEvent CharacterHealthChanged = new CharacterHealthChangedEvent();
}

public class GameEvent
{

}

public class CharacterHealthChangedEvent : GameEvent
{
    public float Value;
    public float MaxValue;
}