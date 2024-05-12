using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Stamina Damage Effect")]
public class TakeStaminaDamageEffect : ScriptableInstantCharacterEffect
{
    public float staminaDamage;

    public override void ProcessEffect(CharacterManager characterEffectable)
    {
        base.ProcessEffect(characterEffectable);
        CalculateStaminaDamage(characterEffectable);
    }

    private void CalculateStaminaDamage(CharacterManager characterEffectable)
    {
        characterEffectable.characterStat.CurrentStamina -= staminaDamage;
    }
}
