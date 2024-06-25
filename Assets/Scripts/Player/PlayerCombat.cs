using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCombat : CharacterCombat, IBackStabable
{
    [Header("Charge Attack")]
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

    [Header("Back Stab")]
    [SerializeField] private BackStabCollider backStabCollider;
    public BoxCollider BackStabCollider => backStabCollider.Collider;

    private bool isBeingBackStabbed = false;
    public bool IsBeingBackStabbed { get => isBeingBackStabbed; set => isBeingBackStabbed = value; }
    public Transform BackStabberTransform => backStabCollider.backStabberTransform;

    [Header("Input")]
    [SerializeField] private ScriptableInputReader inputReader;

    public event Action BackStabed = delegate { };

    private void OnEnable()
    {
        inputReader.LightAttack += OnLightAttack;
        inputReader.HeavyAttack += OnHeavyAttack;
        inputReader.BackStab += OnBackStab;
    }

    private void OnBackStab(bool argument)
    {
        if(!argument || PlayerManager.Instance.playerEquipment.CurrentRightHandWeaponId == 1)
        {
            return;
        }

        RaycastHit hit;
        if(Physics.Raycast(criticalAttackRayCastPoint.position, transform.TransformDirection(Vector3.forward), out hit, criticalAttackLayer))
        {
            IBackStabable backStabable = hit.transform.gameObject.GetComponent<IBackStabable>();
            
            if (backStabable != null && Vector3.Distance(transform.position, hit.transform.position) <= backStabable.BackStabberDistance())
            {

                transform.position = backStabable.BackStabberTransform.position;

                Vector3 direction = transform.root.eulerAngles;
                direction = hit.transform.position - transform.position;
                direction.y = 0;
                direction.Normalize();
                Quaternion rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), 500 * Time.deltaTime);
                transform.rotation = rotation;

                PlayerManager.Instance.playerAnimator.PlayTargetActionAnimation("Backstab_Stab", true);
                hit.transform.gameObject.GetComponent<CharacterManager>().characterAnimator.PlayTargetActionAnimation("Backstab_Stabbed", true);
                backStabable.IsBeingBackStabbed = true;
            }
        }
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

    public float BackStabberDistance()
    {
        return Vector3.Distance(transform.position, BackStabberTransform.transform.position);
    }

    public void ResetIsBeingStabed()
    {
        isBeingBackStabbed = false;
    }
}
