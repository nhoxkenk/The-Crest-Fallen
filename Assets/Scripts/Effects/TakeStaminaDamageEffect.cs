using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Stamina Damage Effect")]
public class TakeStaminaDamageEffect : ScriptableInstantCharacterEffect
{
    public float staminaDamage;

    public override void ProcessEffect(IEffectable characterEffectable)
    {
        base.ProcessEffect(characterEffectable);
        CalculateStaminaDamage(characterEffectable);
    }

    private void CalculateStaminaDamage(IEffectable characterEffectable)
    {
        characterEffectable.TakeInstantStaminaEffect(staminaDamage);
    }
}
