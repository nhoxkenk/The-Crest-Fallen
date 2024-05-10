using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    public DamageCollider DamageCollider { get; set; }

    public void ApplyWeaponDamage(CharacterManager characterManager, WeaponItem weaponItem);
}
