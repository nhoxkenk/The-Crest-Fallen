using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public void HandleLightAttack(WeaponItem weapon)
    {
        PlayerManager.Instance.playerAnimator.PlayTargetActionAnimation(weapon.oneHand_Light_Attack_1, true);
    }

    public void HandleHeavyAttack(WeaponItem weapon)
    {
        PlayerManager.Instance.playerAnimator.PlayTargetActionAnimation(weapon.oneHand_Heavy_Attack_1, true);
    }
}
