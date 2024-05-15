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

    [SerializeField] private ScriptableInputReader inputReader;

    private void OnEnable()
    {
        inputReader.LightAttack += OnLightAttack;
        inputReader.HeavyAttack += OnHeavyAttack;
    }

    private void OnHeavyAttack(bool heavyAttackInput)
    {
        if (heavyAttackInput)
        {
            var inventory = PlayerManager.Instance.playerInventory;

            SetCharacterActionHand(true);
            PerformWeaponBasedAction(inventory.currentRightHandWeapon.rightMouseButtonAction, inventory.currentRightHandWeapon);
        }
    }

    private void OnLightAttack(bool lightAttackInput)
    {
        if(lightAttackInput)
        {
            var inventory = PlayerManager.Instance.playerInventory;

            SetCharacterActionHand(true);
            PerformWeaponBasedAction(inventory.currentRightHandWeapon.leftMouseButtonAction, inventory.currentRightHandWeapon);
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

        float staminaDeducted = 0;

        switch(currentAttackType)
        {
            case AttackType.lightAttack01:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttackStaminaCostMultiplier;
                break;

            default:
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

    private void HandleChargeAttackInput() 
    {
        if (PlayerManager.Instance.isPerformingAction)
        {
            if (isUsingRightHandWeapon)
            {
                IsChargingAttack = inputReader.IsChargingAttack;
            }
        }
    }
}
