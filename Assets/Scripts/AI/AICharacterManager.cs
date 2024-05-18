using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AICharacterManager : CharacterManager
{
    [Header("Current State")]
    [SerializeField] private ScriptableAIState currentState;

    [Header("List of State")]
    public ScriptableAIState idleState;
    public ScriptableAIState pursueState;

    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public AICharacterCombat AICharacterCombat;
    [HideInInspector] public AICharacterLocomotion AICharacterLocomotion;
    protected override void Awake()
    {
        base.Awake();

        agent = GetComponentInChildren<NavMeshAgent>();
        AICharacterCombat = GetComponent<AICharacterCombat>();
        AICharacterLocomotion = GetComponent<AICharacterLocomotion>();
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

        Moving();
    }

    private void ProcessStateMachine()
    {
        ScriptableAIState nextState = currentState?.UpdateState(this);
        if (nextState != null)
        {
            currentState = nextState;
        }
    }

    private void Moving()
    {
        agent.transform.localPosition = Vector3.zero;
        agent.transform.localRotation = Quaternion.identity;

        if (agent.enabled)
        {
            Vector3 destination = agent.destination;
            float remainingDistance = Vector3.Distance(transform.position, destination);
            if(remainingDistance > agent.stoppingDistance)
            {
                IsMoving = true;
            }
            else
            {
                IsMoving = false;
            }
        }
        else
        {
            IsMoving = false;
        }
    }
}
