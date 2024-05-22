using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "States/Combat Stance State")]
public class CombatStanceState : ScriptableAIState
{
    [Header("Attack State")]
    private bool hasAttack;
    public bool HasAttack { get { return hasAttack; } set => hasAttack = value; }

    [Header("Attack")]
    public List<AICharacterAttackAction> allAICharacterAttack;
    private List<AICharacterAttackAction> potentialAttacks;

    private AICharacterAttackAction choosenAttackAction;
    private AICharacterAttackAction previousAttackAction;

    [Header("Combo")]
    [SerializeField] protected bool canPerformCombo = false;
    [SerializeField] protected int chanceToPerformCombo = 25;
    [SerializeField] protected bool hasRolledForComboChance = false;

    [Header("Engagement Distance")]
    [SerializeField] protected float engagementDistance = 5;

    public override ScriptableAIState UpdateState(AICharacterManager aiManager)
    {
        if (aiManager.IsPerformingAction)
        {
            return this;
        }   
            
        if (!aiManager.agent.enabled)
        {
            aiManager.agent.enabled = true;
        }

        if (aiManager.IsMoving)
        {
            if(aiManager.AICharacterCombat.characterViewableAngle < -30 || aiManager.AICharacterCombat.characterViewableAngle > 30)
            {
                aiManager.AICharacterCombat.PivotTowardsTarget(aiManager);
            }
        }

        if (!hasAttack)
        {
            GetNewAttack(aiManager);
        }
        else
        {
            //Check Timer of this AI
            //Pass attack to next state
            //Roll for combo
            //Switch State
        }

        if (aiManager.AICharacterCombat.currentTarget == null)
        {
            return NextState(aiManager, aiManager.idleState);
        }

        if (aiManager.AICharacterCombat.distanceFromTarget > engagementDistance)
        {
            return NextState(aiManager, aiManager.pursueState);
        }

        NavMeshPath path = new NavMeshPath();
        aiManager.agent.CalculatePath(aiManager.AICharacterCombat.currentTarget.transform.position, path);
        aiManager.agent.SetPath(path);

        return this;
    }

    public override ScriptableAIState NextState(AICharacterManager aiManager, ScriptableAIState state)
    {
        return base.NextState(aiManager, state);
    }

    public override void ResetStateFlags(AICharacterManager aiManager)
    {
        base.ResetStateFlags(aiManager);

        hasRolledForComboChance = false;
        hasAttack = false;
    }

    protected virtual void GetNewAttack(AICharacterManager aiManager)
    {
        //sort all attack -> remove all that can't be used -> get remaining attack into list -> pick one based on weight -> pass to Attack State
        potentialAttacks = new List<AICharacterAttackAction>();

        foreach (AICharacterAttackAction potentialAttack in allAICharacterAttack)
        {
            if (potentialAttack.miniumAttackDistance > aiManager.AICharacterCombat.distanceFromTarget ||
                potentialAttack.maxiumAttackDistance < aiManager.AICharacterCombat.distanceFromTarget)
            {
                continue;
            }

            if (potentialAttack.miniumAttackAngle > aiManager.AICharacterCombat.characterViewableAngle ||
                potentialAttack.maxiumAttackAngle < aiManager.AICharacterCombat.characterViewableAngle)
            {
                continue;
            }

            potentialAttacks.Add(potentialAttack);
        }

        if(potentialAttacks.Count <= 0)
        {
            return;
        }

        float totalWeight = 0;
        foreach(var attack in potentialAttacks)
        {
            totalWeight += attack.attackWeight;
        }

        float randomWeight = Random.Range(1, totalWeight + 1);
        float processedWeight = 0;

        foreach(var attack in potentialAttacks)
        {
            processedWeight += attack.attackWeight;
            if(randomWeight < processedWeight)
            {
                previousAttackAction = choosenAttackAction;
                choosenAttackAction = attack;
                hasAttack = true;
            }
        }
    }

    protected virtual bool RollForOutcomeChance(int chance)
    {
        int randomPercent = Random.Range(0, 100);
        return randomPercent < chance ? true : false;
    }
}
