using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCombat : MonoBehaviour
{
    public WeaponItem currentWeaponUsed;

    [Header("Actions Input")]
    [SerializeField] protected bool isUsingLeftHandWeapon;
    public bool IsUsingLeftHandWeapon
    {
        get
        {
            return isUsingLeftHandWeapon;
        }
        set
        {
            isUsingLeftHandWeapon = value;
        }
    }

    [SerializeField] protected bool isUsingRightHandWeapon;
    public bool IsUsingRightHandWeapon
    {
        get
        {
            return isUsingRightHandWeapon;
        }
        set
        {
            isUsingRightHandWeapon = value;
        }
    }

    public virtual void SetCharacterActionHand(bool isRightHand)
    {
        if (isRightHand)
        {
            IsUsingLeftHandWeapon = false;
            IsUsingRightHandWeapon = true;
        }
        else
        {
            IsUsingLeftHandWeapon = true;
            IsUsingRightHandWeapon = false;
        }
    }

    public virtual void PerformWeaponBasedAction(ScriptableWeaponItemAction weaponAction, WeaponItem weapon)
    {
        weaponAction.AttempToPerformAction(PlayerManager.Instance, weapon);

        PerformWeaponAction(weaponAction.actionId, weapon.itemID);
    }

    protected virtual void PerformWeaponAction(int actionId, int weaponId)
    {
        ScriptableWeaponItemAction action = CharacterActionsManager.Instance.GetWeaponItemActionById(actionId);
        WeaponItem weapon = AllItemsManager.Instance.GetWeaponItemById(weaponId);

        if (action != null)
        {
            action.AttempToPerformAction(PlayerManager.Instance, weapon);
        }
    }
}
