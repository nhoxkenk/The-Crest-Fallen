using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableAIState : ScriptableObject
{
    public virtual ScriptableAIState UpdateState(AICharacterManager aiManager)
    {
        return this;
    }

    public virtual ScriptableAIState NextState(AICharacterManager aiManager, ScriptableAIState state)
    {
        ResetStateFlags(aiManager);
        return state;
    }

    public virtual void ResetStateFlags(AICharacterManager aiManager)
    {

    }
}
