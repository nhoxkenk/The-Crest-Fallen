using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableAIState : ScriptableObject
{
    public virtual ScriptableAIState UpdateState(AICharacterManager characterManager)
    {
        return new ScriptableAIState();
    }
}
