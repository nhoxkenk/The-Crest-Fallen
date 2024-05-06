using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private DamageCollider damageCollider;

    private void Awake()
    {
        damageCollider = GetComponentInChildren<DamageCollider>();
    }

    public void SetWeaponDamage(CharacterManager characterManager, WeaponItem weaponItem)
    {
        damageCollider.characterCausingDamage = characterManager;
        damageCollider.physicalDamage = weaponItem.physicalDamage;
        damageCollider.magicDamage = weaponItem.magicDamage;
        damageCollider.fireDamage = weaponItem.fireDamage;
        damageCollider.lightningDamage = weaponItem.lightningDamage;
        damageCollider.holyDamage = weaponItem.holyDamage;
    }
}
