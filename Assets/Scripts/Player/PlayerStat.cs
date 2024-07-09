using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : CharacterStat
{
    [Header("Stamina Regeneration")]
    public float staminaRegenerationAmount = 2;
    public float staminaRegenerationTimerValue = 0.2f;
    public float staminaRegenerationDelayValue = 2;

    private List<Timer> timers;
    public CountdownTimer staminaRegenerateTimer;
    public CountdownTimer staminaRegenerateDelayTimer;

    protected override void Awake()
    {
        base.Awake();
        
        SetupTimer();
    }

    public void OnIncreaseVitalityStat(int oldVitality, int newVitality)
    {
        maxHealth = CalculateHealthBasedOnVitalityLevel(newVitality);
        CurrentHealth = maxHealth;
        PlayerUI.Instance.playerUIHud.HandleMaxHealthValue(maxHealth);
    }

    public void OnIncreaseEnduranceStat(int oldEndurance, int newEndurance)
    {
        maxStamina = CalculateStaninaBasedOnEnduranceLevel(newEndurance);
        CurrentStamina = maxStamina;
        PlayerUI.Instance.playerUIHud.HandleMaxStaminaValue(maxStamina);
    }

    private void SetupTimer()
    {
        staminaRegenerateTimer = new CountdownTimer(staminaRegenerationTimerValue);
        staminaRegenerateDelayTimer = new CountdownTimer(staminaRegenerationDelayValue);
        timers = new List<Timer>(2) { staminaRegenerateTimer, staminaRegenerateDelayTimer};
    }

    public void ResetStaminaRegenerationTimer(float oldValue, float newValue)
    {
        if (!staminaRegenerateDelayTimer.IsRunning)
        {
            staminaRegenerateDelayTimer.Start();
        }
        
        if (newValue < oldValue)
        {
            staminaRegenerateDelayTimer.Reset();
        }
    }

    public void RegenerateStamina()
    {
        if (PlayerManager.Instance.playerLocomotion.IsSprinting || PlayerManager.Instance.IsPerformingAction)
        {
            return;
        }

        staminaRegenerateDelayTimer.Tick(Time.deltaTime);

        if (staminaRegenerateDelayTimer.IsFinished())
        {
            if (CurrentStamina < maxStamina)
            {
                staminaRegenerateTimer.Tick(Time.deltaTime);
                if (staminaRegenerateTimer.IsFinished())
                {
                    staminaRegenerateTimer.Reset();
                    staminaRegenerateTimer.Start();
                    CurrentStamina += staminaRegenerationAmount;
                }
            }
        }

        OnRegeneratingStamina();
    }

}
