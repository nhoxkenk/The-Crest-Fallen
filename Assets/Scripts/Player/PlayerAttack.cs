using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerManager playerManager;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
    }

    public void HandleLightAttack(WeaponItem weapon)
    {
        playerManager.playerAnimator.PlayTargetActionAnimation(weapon.oneHand_Light_Attack_1, true);
    }

    public void HandleHeavyAttack(WeaponItem weapon)
    {
        playerManager.playerAnimator.PlayTargetActionAnimation(weapon.oneHand_Heavy_Attack_1, true);
    }
}
