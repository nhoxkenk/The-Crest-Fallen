using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Attack State")]
public class AttackState : ScriptableAIState
{
    [Header("Current Attack")]
    public AICharacterAttackAction currentAttack;
    public bool hasFollowUpAttack;  //Or Combo Chain for Attack

    [Header("State Flags")]
    protected bool hasPerformedAttack;
    protected bool hasPerformedFollowUpAttack;

    [Header("Options")]
    [SerializeField] protected bool pivotAfterAttack;

    public override ScriptableAIState UpdateState(AICharacterManager aiManager)
    {
        if(aiManager.AICharacterCombat.currentTarget == null || !aiManager.AICharacterCombat.currentTarget.IsAlive)
        {
            return NextState(aiManager, aiManager.idleState);
        }

        aiManager.AICharacterCombat.RotateTowardsTargetWhileAttack(aiManager);

        //Rotate toward target when attacking
        aiManager.characterAnimator.UpdateAnimatorMovementParameters(0, 0, false);

        //has a follow up attack and not perform it
        if(hasFollowUpAttack && !hasPerformedFollowUpAttack)
        {
            if(currentAttack.comboAction != null)
            {
                //if can combo
                //hasPerformedFollowUpAttack = true;
                //currentAttack.comboAction.AttempToPerformAttackAction(aiManager);
            }
        }

        if ((aiManager.IsPerformingAction))
        {
            return this;
        }

        if (!hasPerformedAttack)
        {
            if (aiManager.AICharacterCombat.actionTimer.IsRunning || aiManager.IsPerformingAction)
            {
                return this;
            }

            PerformAttack(aiManager);
            return this;
        }

        if(pivotAfterAttack)
        {
            aiManager.AICharacterCombat.PivotTowardsTarget(aiManager);
        }

        return NextState(aiManager, aiManager.combatStanceState);
    }

    protected void PerformAttack(AICharacterManager aiManager)
    {
        hasPerformedAttack = true;
        currentAttack.AttempToPerformAttackAction(aiManager);
        aiManager.AICharacterCombat.actionTimer.Reset(currentAttack.actionRecoveryTime);
        aiManager.AICharacterCombat.actionTimer.Start();
    }

    public override void ResetStateFlags(AICharacterManager aiManager)
    {
        base.ResetStateFlags(aiManager);

        hasPerformedAttack = false;
        hasPerformedFollowUpAttack = false;
    }
}
