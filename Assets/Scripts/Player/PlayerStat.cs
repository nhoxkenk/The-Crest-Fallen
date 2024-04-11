using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : CharacterStat
{
    private PlayerManager playerManager;
    public event Action<float, float> OnStaminaDrain;
    public event Action<float, float> OnStaminaRegenerate;
    [Header("Stamina Regeneration")]
    public float staminaRegenerationAmount = 2;
    public float staminaRegenerationTimer = 0;
    public float staminaRegenerationDelay = 2;
    [SerializeField] private float staminaTickTimer = 0;
    
    protected override void Awake()
    {
        base.Awake();
        InitializePlayerStat();
    }

    private void InitializePlayerStat()
    {
        playerManager = GetComponent<PlayerManager>();
        maxStamina = CalculateStaninaBasedOnEnduranceLevel(endurance);
        PlayerUI.Instance.playerUIHud.SetMaxStaminaValue(maxStamina);
        currentStamina = maxStamina;
    }

    public void DrainStaminaOnSprinting()
    {
        currentStamina -= playerManager.playerLocomotion.sprintingStaminaCost * Time.deltaTime;
        OnStaminaDrain?.Invoke(maxStamina, currentStamina);
    }

    public void RegenerateStamina()
    {
        if (playerManager.playerLocomotion.IsSprinting || playerManager.isPerformingAction)
        {
            return;
        }
        
        staminaRegenerationTimer += Time.deltaTime;

        if(staminaRegenerationTimer >= staminaRegenerationDelay)
        {
            if(playerManager.playerStat.currentStamina < playerManager.playerStat.maxStamina)
            {
                staminaTickTimer += Time.deltaTime;

                if(staminaTickTimer >= 0.1)
                {
                    staminaTickTimer = 0;
                    playerManager.playerStat.currentStamina += staminaRegenerationAmount;
                }
            }
        }
        OnStaminaRegenerate?.Invoke(maxStamina, currentStamina);
    }

}
