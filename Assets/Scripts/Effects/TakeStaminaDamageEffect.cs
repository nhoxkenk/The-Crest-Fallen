using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Stamina Damage Effect")]
public class TakeStaminaDamageEffect : ScriptableInstantCharacterEffect
{
    public float staminaDamage;

    public override void ProcessEffect(PlayerManager playerManager)
    {
        base.ProcessEffect(playerManager);
        CalculateStaminaDamage(playerManager);
    }

    private void CalculateStaminaDamage(PlayerManager playerManager)
    {
        playerManager.playerStat.DrainStaminaBasedOnAction(Mathf.RoundToInt(staminaDamage), false);
    }
}
