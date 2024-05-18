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

        aiManager.AICharacterLocomotion.RotateTowardTarget(aiManager);

        NavMeshPath path = new NavMeshPath();
        aiManager.agent.CalculatePath(aiManager.AICharacterCombat.currentTarget.transform.position, path);
        aiManager.agent.SetPath(path);

        return this;
    }
}
