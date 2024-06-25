using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Test Action")]
public class ScriptableWeaponItemAction : ScriptableObject
{
    public int actionId;

    public virtual void AttempToPerformAction(CharacterManager characterPerformAction, WeaponItem weaponPerformAction)
    {
        characterPerformAction.characterEquipment.CurrentWeaponBeingUsedId = weaponPerformAction.itemID;
    }
}
