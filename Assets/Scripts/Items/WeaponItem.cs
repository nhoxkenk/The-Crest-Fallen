using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : ScriptableItem
{
    public bool isUnarmed;

    [Header("Weapon Model")]
    public GameObject weaponModelPrefab;

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

    [Header("Weapon Base Damage")]
    public int baseStaminaCost = 20;

    [Header("Actions")]
    public ScriptableWeaponItemAction leftMouseButtonAction;
}
