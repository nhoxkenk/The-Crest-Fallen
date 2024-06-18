using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : ScriptableItem
{
    public bool isUnarmed;

    [Header("Weapon Model")]
    public GameObject weaponModelPrefab;

    [Header("Stamina Cost")]
    public int baseStaminaCost = 20;

    [Header("Weapon Requirements")]
    public int strengthREQ = 0;
    public int dexREQ = 0;
    public int intREQ = 0;
    public int faithREQ = 0;

    [Header("Weapon Base Damage")]
    public int physicalDamage;
    public int magicDamage;
    public int fireDamage;
    public int lightningDamage;
    public int holyDamage;

    [Header("Weapon Poise")]
    public int poiseDamage = 5;

    [Header("Attack Modifiers")]
    public float lightAttack01Modifier = 1.1f;
    public float heavyAttack01Modifier = 1.5f;
    public float chargeAttack01Modifier = 2.0f;

    [Header("Stamina Modifiers")]
    public float lightAttackStaminaCostMultiplier = 0.9f;
    public float heavyAttackStaminaCostMultiplier = 1.1f;
    public float chargeAttackStaminaCostMultiplier = 1.2f;

    [Header("Actions")]
    public ScriptableWeaponItemAction leftMouseButtonAction;
    public ScriptableWeaponItemAction rightMouseButtonAction;

    public override int UseItemAndReturnId()
    {
        characterHoldThisItem.characterEquipment.EquipWeapon(this);
        return base.UseItemAndReturnId();
    }
}
