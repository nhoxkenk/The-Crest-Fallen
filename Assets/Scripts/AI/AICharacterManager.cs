using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacterManager : CharacterManager
{
    [Header("Current State")]
    [SerializeField] private ScriptableAIState currentState;

    [HideInInspector] public AICharacterCombat AICharacterCombat;

    protected override void Awake()
    {
        base.Awake();

        AICharacterCombat = GetComponent<AICharacterCombat>();

    }

    protected override void Start()
    {
        base.Start();
        BindingCharacterEvents();
        InitializeStat();
    }

    private void BindingCharacterEvents()
    {
        characterStat.CurrentHealthChange += characterStat.HandleCurrentHealthChange;
    }

    /// <summary>
    /// Initialize Character Stat
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

    protected override void FixedUpdate()
    {
        ProcessStateMachine();
    }

    private void ProcessStateMachine()
    {
        if(currentState != null)
        {
            currentState.UpdateState(this);
        }
    }
}
