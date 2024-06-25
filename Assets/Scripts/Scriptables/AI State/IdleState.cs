using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Idle State")]
public class IdleState : ScriptableAIState
{
    public override ScriptableAIState UpdateState(AICharacterManager characterManager)
    {
        if(characterManager.AICharacterCombat.currentTarget != null)
        {
            return NextState(characterManager, characterManager.pursueState);
        }
        else
        {
            characterManager.AICharacterCombat.FindTargetViaLineOfSight(characterManager);
            return this;
        }
    }
}
