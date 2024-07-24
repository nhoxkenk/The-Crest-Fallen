using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Effects/Instant Effects/Increase Character Max Stat Effect")]
public class IncreaseCharacterMaxStat : ScriptableInstantCharacterEffect
{
    [SerializeField] private float ValueIncrease;

    private void OnEnable()
    {
        OnProcessEffect += IncreaseCharacterStat;
    }

    private void OnDisable()
    {
        OnProcessEffect -= IncreaseCharacterStat;
    }

    public override void ProcessEffect(CharacterManager characterEffectable)
    {
        base.ProcessEffect(characterEffectable);
    }

    private void IncreaseCharacterStat(CharacterManager characterEffectable)
    {
        if(effectType == EffectType.EffectState)
        {
            return;
        }

        if (statEffected == BaseStatType.Health)
        {
            characterEffectable.characterStat.Vitality += (int)ValueIncrease;
        }
        else if (statEffected == BaseStatType.Stamina)
        {
            characterEffectable.characterStat.Endurance += (int)ValueIncrease;
        }
    }

    public void RemoveCharacterStat(CharacterManager characterEffectable)
    {
        if (effectType == EffectType.EffectState)
        {
            return;
        }

        if (statEffected == BaseStatType.Health)
        {
            characterEffectable.characterStat.Vitality -= (int)ValueIncrease;
        }
        else if (statEffected == BaseStatType.Stamina)
        {
            characterEffectable.characterStat.Endurance -= (int)ValueIncrease;
        }
    }
}
