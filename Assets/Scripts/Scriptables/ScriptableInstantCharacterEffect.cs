using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableInstantCharacterEffect : ScriptableObject
{
    [Header("Effect ID")]
    public int instantEffectID;

    public virtual void ProcessEffect(CharacterManager characterManager)
    {

    }
}
