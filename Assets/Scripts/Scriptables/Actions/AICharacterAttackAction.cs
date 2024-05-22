using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/AI Actions/Attack Action")]
public class AICharacterAttackAction : ScriptableObject
{
    [Header("Name Attack Perform")]
    [SerializeField] private string nameAttackAnimation;

    [Header("Combo Actions")]
    public AICharacterAttackAction comboAction;

    [Header("Action Values")]
    [SerializeField] private AttackType attackType;
    public int attackWeight = 50;

    public float actionRecoveryTime = 1.5f;
    public float miniumAttackAngle = -35;
    public float maxiumAttackAngle = 35;
    public float miniumAttackDistance = 0;
    public float maxiumAttackDistance = 2;

    public void AttempToPerformAttackAction(AICharacterManager aiCharacterManager)
    {
        aiCharacterManager.characterAnimator.PlayTargetAttackAnimation(attackType, nameAttackAnimation, true);
    }
}
