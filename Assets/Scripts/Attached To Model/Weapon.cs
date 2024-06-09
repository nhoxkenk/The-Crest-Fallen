using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour, IWeapon
{
    public DamageCollider damageCollider;

    public DamageCollider DamageCollider { get => damageCollider; set => damageCollider = value; }

    private void Awake()
    {
        damageCollider = GetComponentInChildren<DamageCollider>();
    }

    public void ApplyWeaponDamage(CharacterManager characterManager, WeaponItem weaponItem)
    {
        damageCollider.characterCausingDamage = characterManager;
        damageCollider.physicalDamage = weaponItem.physicalDamage;
        damageCollider.magicDamage = weaponItem.magicDamage;
        damageCollider.fireDamage = weaponItem.fireDamage;
        damageCollider.lightningDamage = weaponItem.lightningDamage;
        damageCollider.holyDamage = weaponItem.holyDamage;

        var modifiers = damageCollider.GetComponent<IWeaponModifier>();

        modifiers.LightAttack01Modifier = weaponItem.lightAttack01Modifier;
        modifiers.HeavyAttack01Modifier = weaponItem.heavyAttack01Modifier;
        modifiers.ChargeAttack01Modifier = weaponItem.chargeAttack01Modifier;
    }
}
