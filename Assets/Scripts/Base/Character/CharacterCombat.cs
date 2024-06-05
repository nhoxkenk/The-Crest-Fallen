using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCombat : MonoBehaviour
{
    [HideInInspector] private CharacterManager character;

    [Header("Last Attack Animation")]
    public string lastAttackAnimation;

    public WeaponItem currentWeaponBeingUsed;
    public AttackType currentAttackType;

    [Header("Target")]
    public CharacterManager currentTarget;

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

    [Header("Flags")]
    public bool canComboWithMainHandWeapon;

    private void Awake()
    {
        character = GetComponent<CharacterManager>();
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

    public virtual void DrainStaminaBaseOnWeaponAction()
    {

    }

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
        PlayerManager.Instance.characterSoundEffect.PlaySoundFX(SoundEffectsManager.Instance.PlayRandomSwingSoundEffect());
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

    public virtual void SetTarget(CharacterManager target)
    {
        if(target != null)
        {
            currentTarget = target;
        }
        else
        {
            currentTarget = null;
        }
    }

    public void EnableIsInvulnerable()
    {
        character.IsInvulnerable = true;
    }

    public void DisableIsInvulnerable()
    {
        character.IsInvulnerable = false;
    }
}
