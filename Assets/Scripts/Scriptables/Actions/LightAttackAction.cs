using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Light Attack Action")]
public class LightAttackAction : ScriptableWeaponItemAction
{
    [SerializeField] private string lightAttack_01 = "Main_Light_Attack_01";
    public override void AttempToPerformAction(CharacterManager characterPerformAction, WeaponItem weaponPerformAction)
    {
        base.AttempToPerformAction(characterPerformAction, weaponPerformAction);

        if(characterPerformAction.characterStat.CurrentStamina <= 0 || !characterPerformAction.isGrounded)
        {
            return;
        }

        PerformLightAttack(characterPerformAction, weaponPerformAction);
    }

    private void PerformLightAttack(CharacterManager characterPerformingAction, WeaponItem weaponPerformingAction)
    {
        if (characterPerformingAction.characterCombat.IsUsingLeftHandWeapon)
        {

        }

        if (characterPerformingAction.characterCombat.IsUsingRightHandWeapon)
        {
            characterPerformingAction.characterAnimator.PlayTargetAttackAnimation(AttackType.lightAttack01, lightAttack_01, true);
        }
    }
}
