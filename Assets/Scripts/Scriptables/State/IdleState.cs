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
            Debug.Log("Found A Target");
            return this;
        }
        else
        {
            Debug.Log("Not Found A Target");
            characterManager.AICharacterCombat.FindTargetViaLineOfSight(characterManager);
            return this;
        }
    }
}
