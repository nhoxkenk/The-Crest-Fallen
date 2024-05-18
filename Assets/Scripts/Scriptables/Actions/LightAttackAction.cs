using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Light Attack Action")]
public class LightAttackAction : ScriptableWeaponItemAction
{
    [SerializeField] private string lightAttack_01 = "Main_Light_Attack_01";
    [SerializeField] private string lightAttack_02 = "Main_Light_Attack_02";

    public override void AttempToPerformAction(CharacterManager characterPerformAction, WeaponItem weaponPerformAction)
    {
        base.AttempToPerformAction(characterPerformAction, weaponPerformAction);

        if(characterPerformAction.characterStat.CurrentStamina <= 0 || !characterPerformAction.IsGrounded)
        {
            return;
        }

        PerformLightAttack(characterPerformAction, weaponPerformAction);
    }

    private void PerformLightAttack(CharacterManager characterPerformingAction, WeaponItem weaponPerformingAction)
    {
        if (characterPerformingAction.IsPerformingAction && characterPerformingAction.characterCombat.canComboWithMainHandWeapon)
        {
            characterPerformingAction.characterCombat.canComboWithMainHandWeapon = false;
            if(characterPerformingAction.characterCombat.lastAttackAnimation == lightAttack_01)
            {
                characterPerformingAction.characterAnimator.PlayTargetAttackAnimation(AttackType.lightAttack02, lightAttack_02, true);
            }
            else
            {
                characterPerformingAction.characterAnimator.PlayTargetAttackAnimation(AttackType.lightAttack01, lightAttack_01, true);
            }
        }
        else if(!characterPerformingAction.IsPerformingAction)
        {
            characterPerformingAction.characterAnimator.PlayTargetAttackAnimation(AttackType.lightAttack01, lightAttack_01, true);
        }
    }
}
