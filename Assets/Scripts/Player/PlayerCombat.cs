using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCombat : CharacterCombat
{
    [SerializeField] private bool isChargingAttack;
    private bool IsChargingAttack 
    { 
        get { return isChargingAttack; }
        set
        {
            isChargingAttack = value;
            OnIsChargingAttack?.Invoke(IsChargingAttack);
        }
    }
    public event UnityAction<bool> OnIsChargingAttack;

    public bool CanAttack { get; set; } = true;

    [SerializeField] private ScriptableInputReader inputReader;

    private void OnEnable()
    {
        inputReader.LightAttack += OnLightAttack;
        inputReader.HeavyAttack += OnHeavyAttack;
    }

    private void OnHeavyAttack(bool heavyAttackInput)
    {
        if (heavyAttackInput && CanAttack)
        {
            var inventory = PlayerManager.Instance.playerInventory;

            SetCharacterActionHand(true);
            PerformWeaponBasedAction(inventory.currentRightHandWeapon.rightMouseButtonAction, inventory.currentRightHandWeapon);
        }
    }

    private void OnLightAttack(bool lightAttackInput)
    {
        if(lightAttackInput && CanAttack)
        {
            var inventory = PlayerManager.Instance.playerInventory;

            SetCharacterActionHand(true);
            PerformWeaponBasedAction(inventory.currentRightHandWeapon.leftMouseButtonAction, inventory.currentRightHandWeapon);
        }
    }

    private void HandleChargeAttackInput()
    {
        if (PlayerManager.Instance.IsPerformingAction && CanAttack)
        {
            if (isUsingRightHandWeapon)
            {
                IsChargingAttack = inputReader.IsChargingAttack;
            }
        }
    }

    /// <summary>
    /// This function is register as an event in Animation events of Light Attack Animation.
    /// </summary>
    public override void DrainStaminaBaseOnWeaponAction()
    {
        if(currentWeaponBeingUsed == null)
        {
            return;
        }

        float staminaDeducted;

        switch(currentAttackType)
        {
            case AttackType.lightAttack01:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttackStaminaCostMultiplier;
                break;
            case AttackType.lightAttack02:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttackStaminaCostMultiplier;
                break;
            case AttackType.HeavyAttack01:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.heavyAttackStaminaCostMultiplier;
                break;
            case AttackType.HeavyAttack02:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.heavyAttackStaminaCostMultiplier;
                break;
            case AttackType.ChargeAttack01:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.chargeAttackStaminaCostMultiplier;
                break;
            case AttackType.ChargeAttack02:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.chargeAttackStaminaCostMultiplier;
                break;
            default:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost;
                break;
        }

        PlayerManager.Instance.playerStat.CurrentStamina -= staminaDeducted;
    }

    public void HandleAllHoldingInputAction()
    {
        HandleChargeAttackInput();
    }

    public void HandleIsLockOnChanged(bool value)
    {
        if(!value)
        {
            SetTarget(null);
        }
    }

    public override void SetTarget(CharacterManager target)
    {
        base.SetTarget(target);
        PlayerCamera.Instance.SetLockOnCameraHeight();
    }
}
