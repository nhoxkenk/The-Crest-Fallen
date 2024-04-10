using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : CharacterStat
{
    private PlayerManager playerManager;
    public event Action<float, float> OnStaminaDrain;
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
}
