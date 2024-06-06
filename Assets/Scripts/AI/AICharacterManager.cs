using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AICharacterManager : CharacterManager
{
    [Header("Current State")]
    [SerializeField] protected ScriptableAIState currentState;

    [Header("List of State")]
    public ScriptableAIState idleState;
    public ScriptableAIState pursueState;
    public ScriptableAIState combatStanceState;
    public AttackState attackState;

    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public AICharacterCombat AICharacterCombat;
    [HideInInspector] public AICharacterLocomotion AICharacterLocomotion;

    protected override void Awake()
    {
        base.Awake();

        GetComponents();
    }

    protected override void Start()
    {
        base.Start();
        BindingCharacterEvents();
        InitializeStat();
    }

    protected override void Update()
    {
        base.Update();

        AICharacterCombat.HandleActionTimer(this);
    }

    protected override void GetComponents()
    {
        base.GetComponents();

        agent = GetComponentInChildren<NavMeshAgent>();
        AICharacterCombat = GetComponent<AICharacterCombat>();
        AICharacterLocomotion = GetComponent<AICharacterLocomotion>();
    }

    protected virtual void BindingCharacterEvents()
    {
        characterStat.CurrentHealthChange += characterStat.HandleCurrentHealthChange;

        idleState = Instantiate(idleState);
        pursueState = Instantiate(pursueState);
        combatStanceState = Instantiate(combatStanceState);
        attackState = Instantiate(attackState);
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

        //Get the Angle needed for animtion turn, but right now the animation set not working
        if(AICharacterCombat.currentTarget != null)
        {
            AICharacterCombat.targetDirection = AICharacterCombat.currentTarget.transform.position - transform.position;
            AICharacterCombat.characterViewableAngle = AICharacterCombat.GetAngleOfTarget(this.transform);
            AICharacterCombat.distanceFromTarget = Vector3.Distance(transform.position, AICharacterCombat.currentTarget.transform.position);
        }

        if (agent.enabled && currentState != idleState)
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
