using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Stamina Damage Effect")]
public class TakeStaminaDamageEffect : ScriptableInstantCharacterEffect
{
    public float staminaDamage;

    public override void ProcessEffect(CharacterManager characterManager)
    {
        base.ProcessEffect(characterManager);
        CalculateStaminaDamage(characterManager);
    }

    private void CalculateStaminaDamage(CharacterManager characterManager)
    {
        //characterManager.characterStat.OnDrainStaminaBasedOnAction(Mathf.RoundToInt(staminaDamage), false);
        characterManager.characterStat.CurrentStamina -= staminaDamage;
    }
}
