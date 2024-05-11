using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterCombat : MonoBehaviour
{
    public WeaponItem currentWeaponBeingUsed;
    public AttackType currentAttackType;

    [Header("Target")]
    public CharacterManager currentTargetManager;

    [Header("Lock On transform")]
    public Transform lockOnTransform;

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
    }

    public abstract void DrainStaminaBaseOnWeaponAction();

    /// <summary>
    /// Register as an event to Animations Events
    /// </summary>
    public void OpenWeaponDamageCollider()
    {
        if (isUsingLeftHandWeapon)
        {
            PlayerManager.Instance.playerEquipment.leftHandWeaponManager.DamageCollider.EnableDamageCollider();
        }
        if (isUsingRightHandWeapon)
        {
            PlayerManager.Instance.playerEquipment.rightHandWeaponManager.DamageCollider.EnableDamageCollider();
        }
    }
    /// <summary>
    /// Register as an event to Animations Events
    /// </summary>
    public void CloseWeaponDamageCollider()
    {
        if (isUsingLeftHandWeapon)
        {
            PlayerManager.Instance.playerEquipment.leftHandWeaponManager.DamageCollider.DisableDamageCollider();
        }
        if (isUsingRightHandWeapon)
        {
            PlayerManager.Instance.playerEquipment.rightHandWeaponManager.DamageCollider.DisableDamageCollider();
        }
    }
}
