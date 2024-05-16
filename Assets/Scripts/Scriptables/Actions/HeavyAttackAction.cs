using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Heavy Attack Action")]

public class HeavyAttackAction : ScriptableWeaponItemAction
{
    [SerializeField] private string heavyAttack_01 = "Main_Heavy_Attack_01";
    [SerializeField] private string heavyAttack_02 = "Main_Heavy_Attack_02";

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

        if (characterPerformingAction.isPerformingAction && characterPerformingAction.characterCombat.canComboWithMainHandWeapon)
        {
            characterPerformingAction.characterCombat.canComboWithMainHandWeapon = false;
            if (characterPerformingAction.characterCombat.lastAttackAnimation == heavyAttack_01)
            {
                characterPerformingAction.characterAnimator.PlayTargetAttackAnimation(AttackType.HeavyAttack02, heavyAttack_02, true);
            }
            else
            {
                characterPerformingAction.characterAnimator.PlayTargetAttackAnimation(AttackType.HeavyAttack01, heavyAttack_01, true);
            }
        }
        else if (!characterPerformingAction.isPerformingAction)
        {
            characterPerformingAction.characterAnimator.PlayTargetAttackAnimation(AttackType.HeavyAttack01, heavyAttack_01, true);
        }
    }
}
