using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScriptableInstantCharacterEffect : ScriptableObject
{
    [Header("Effect ID")]
    public int instantEffectID;

    [Header("Type")]
    public EffectType effectType;
    public BaseStatType statEffected;

    public event Action<CharacterManager> OnProcessEffect;

    public virtual void ProcessEffect(CharacterManager characterEffectable) 
    {
        OnProcessEffect?.Invoke(characterEffectable);
    }
}
