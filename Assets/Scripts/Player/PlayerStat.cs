using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : CharacterStat
{
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
        maxStamina = CalculateStaninaBasedOnEnduranceLevel(endurance);
        PlayerUI.Instance.playerUIHud.HandleMaxStaminaValue(maxStamina);
        currentStamina = maxStamina;
    }

    public override void OnDrainStaminaBasedOnAction(int stamina, bool isContinuous)
    {
        base.OnDrainStaminaBasedOnAction(stamina, isContinuous); 
    }

    public void RegenerateStamina()
    {
        if (PlayerManager.Instance.playerLocomotion.IsSprinting || PlayerManager.Instance.isPerformingAction)
        {
            return;
        }
        
        staminaRegenerationTimer += Time.deltaTime;

        if(staminaRegenerationTimer >= staminaRegenerationDelay)
        {
            if(PlayerManager.Instance.playerStat.currentStamina < PlayerManager.Instance.playerStat.maxStamina)
            {
                staminaTickTimer += Time.deltaTime;

                if(staminaTickTimer >= 0.1)
                {
                    staminaTickTimer = 0;
                    PlayerManager.Instance.playerStat.currentStamina += staminaRegenerationAmount;
                }
            }
        }
        OnRegeneratingStamina();
    }

}
