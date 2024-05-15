using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Heavy Attack Action")]

public class HeavyAttackAction : ScriptableWeaponItemAction
{
    [SerializeField] private string heavyAttack_01 = "Main_Heavy_Attack_01";

    public override void AttempToPerformAction(CharacterManager characterPerformAction, WeaponItem weaponPerformAction)
    {
        base.AttempToPerformAction(characterPerformAction, weaponPerformAction);

        if (characterPerformAction.characterStat.CurrentStamina <= 0 || !characterPerformAction.isGrounded)
        {
            return;
        }

        PerformHeavyAttack(characterPerformAction, weaponPerformAction);
    }

    private void PerformHeavyAttack(CharacterManager characterPerformingAction, WeaponItem weaponPerformingAction)
    {
        if (characterPerformingAction.characterCombat.IsUsingLeftHandWeapon)
        {

        }

        if (characterPerformingAction.characterCombat.IsUsingRightHandWeapon)
        {
            characterPerformingAction.characterAnimator.PlayTargetAttackAnimation(AttackType.HeavyAttack01, heavyAttack_01, true);
        }
    }
}
