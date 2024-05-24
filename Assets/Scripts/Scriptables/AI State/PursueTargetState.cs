using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "States/Pursue State")]
public class PursueTargetState : ScriptableAIState
{
    public override ScriptableAIState UpdateState(AICharacterManager aiManager)
    {
        if (aiManager.AICharacterCombat.currentTarget == null)
        {
            return NextState(aiManager, aiManager.idleState);
        }

        if (aiManager.IsPerformingAction)
        {
            return this;
        }

        if (!aiManager.agent.enabled)
        {
            aiManager.agent.enabled = true;
        }

        //Option pivot to face the target if it out of view

        aiManager.AICharacterLocomotion.RotateTowardTarget(aiManager);

        if(aiManager.AICharacterCombat.distanceFromTarget <= aiManager.agent.stoppingDistance)
        {
            return NextState(aiManager, aiManager.combatStanceState);
        }

        NavMeshPath path = new NavMeshPath();
        aiManager.agent.CalculatePath(aiManager.AICharacterCombat.currentTarget.transform.position, path);
        aiManager.agent.SetPath(path);

        return this;
    }
}
