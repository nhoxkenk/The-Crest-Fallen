using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemFactory : ScriptableObject
{
    public abstract IWeapon CreateAndGetWeapon(CharacterManager characterHoldItem, WeaponItem weaponItem);

    public abstract GameObject GetWeaponModel();
}
