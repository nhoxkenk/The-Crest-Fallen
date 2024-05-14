using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : CharacterManager
{
    [Header("Debug")]
    public bool revive;
    public bool switchRightWeapon;

    protected override void Awake()
    {
        base.Awake();

        GetComponents();
    }

    protected override void Start()
    {
        base.Start();
        BindingPlayerEvents();
        InitializeStat();
    }

    private void BindingPlayerEvents()
    {
        characterStat.CurrentHealthChange += characterStat.HandleCurrentHealthChange;
    }

    protected override void Update()
    {
        base.Update();
        DebugMenu();
    }

    private void LateUpdate()
    {
        //PlayerCamera.Instance.HandleAllCameraActions();
    }

    /// <summary>
    /// Initialize Player Stat
    /// </summary>
    private void InitializeStat()
    {
        //Stamina
        characterStat.maxStamina = characterStat.CalculateStaninaBasedOnEnduranceLevel(characterStat.Endurance);
        characterStat.CurrentStamina = characterStat.maxStamina;
        //Health
        characterStat.maxHealth = characterStat.CalculateHealthBasedOnVitalityLevel(characterStat.Vitality);
        characterStat.CurrentHealth = characterStat.maxHealth;
    }

    public override IEnumerator ProcessDeathEvent(bool manualSelectDeathAnimation = false)
    {
        return base.ProcessDeathEvent(manualSelectDeathAnimation);
    }

    public override void ReviveCharacter()
    {
        base.ReviveCharacter();

        characterStat.CurrentHealth = characterStat.maxHealth;
        characterStat.CurrentStamina = characterStat.maxStamina;

        characterAnimator.PlayTargetActionAnimation("Empty", false);

        IsAlive = true;
    }

    private void DebugMenu()
    {
        if (revive)
        {
            revive = false;
            ReviveCharacter();
        }

        if (switchRightWeapon)
        {
            switchRightWeapon = false;
        }
    }
}
