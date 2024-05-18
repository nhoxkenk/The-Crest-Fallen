using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacterLocomotion : CharacterLocomotion
{
    public void RotateTowardTarget(AICharacterManager aiCharacterManager)
    {
        if (aiCharacterManager.IsMoving)
        {
            aiCharacterManager.transform.rotation = aiCharacterManager.agent.transform.rotation;
        }
    }
}
