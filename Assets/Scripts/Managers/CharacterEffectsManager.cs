using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffectsManager : Singleton<CharacterEffectsManager>
{
    [SerializeField] private List<ScriptableInstantCharacterEffect> scriptableInstantCharacterEffects;

    [Header("Damage")]
    public TakeHealthDamageEffect takeDamageEffect;

    [Header("Restore")]
    public RestoreHealthEffect restoreHealthEffect;

    [Header("VFX")]
    public GameObject bloodSplatVFX;

    protected override void Awake()
    {
        base.Awake();

        GenerateEffectsID();
    }

    private void GenerateEffectsID()
    {
        for(int i = 0; i < scriptableInstantCharacterEffects.Count; i++)
        {
            scriptableInstantCharacterEffects[i].instantEffectID = i;
        }
    }
}
