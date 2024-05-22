using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacterCombat : CharacterCombat
{
    [Header("Target Infomations")]
    public float distanceFromTarget;
    public Vector3 targetDirection;

    [Header("Detection Range")]
    public float characterViewableAngle;

    [SerializeField] private float detectionRadius = 15f;
    [SerializeField] private float miniumDetectionAngle = -25;
    [SerializeField] private float maxiumDetectionAngle = 35;

    public override void DrainStaminaBaseOnWeaponAction()
    {
        //noop
    }

    public void FindTargetViaLineOfSight(AICharacterManager characterFinding)
    {
        if(currentTarget != null)
        {
            return;
        }

        Collider[] colliders = Physics.OverlapSphere(characterFinding.transform.position, detectionRadius, CharacterLayersManager.Instance.CharacterLayerMask);

        foreach(Collider collider in colliders)
        {
            CharacterManager targetCharacter = collider.GetComponent<CharacterManager>();

            if(targetCharacter == null) 
                continue;

            if(!targetCharacter.IsAlive || targetCharacter == characterFinding)
            {
                continue;
            }

            if (characterFinding.CanDealDamageTo(targetCharacter))
            {
                Vector3 targetDistance = characterFinding.transform.position - targetCharacter.transform.position;
                float viewableAngle = Vector3.Angle(targetDistance, targetCharacter.transform.position);
                
                if(viewableAngle < miniumDetectionAngle || viewableAngle > maxiumDetectionAngle)
                {
                    continue;
                }

                if (Physics.Linecast(characterFinding.characterCombat.lockOnTransform.position, targetCharacter.characterCombat.lockOnTransform.position, CharacterLayersManager.Instance.EnvironmentLayerMask))
                {
                    Debug.Log("Block By Environment");
                }
                else
                {
                    targetDirection = targetCharacter.transform.position - transform.position;
                    characterViewableAngle = GetAngleOfTarget(this.transform);
                    characterFinding.characterCombat.SetTarget(targetCharacter);
                    PivotTowardsTarget(characterFinding);                    
                }
            }
        }
    }
    
    public void PivotTowardsTarget(AICharacterManager character)
    {
        if (character.IsPerformingAction)
        {
            return;
        }

        if(characterViewableAngle >= 40 && characterViewableAngle <= 70)
        {
            character.characterAnimator.PlayTargetActionAnimation("Turn_Right", true);
        }

        if (characterViewableAngle <= -40 && characterViewableAngle >= -70)
        {
            character.characterAnimator.PlayTargetActionAnimation("Turn_Left", true);
        }

        if (characterViewableAngle >= 145 && characterViewableAngle <= 180)
        {
            character.characterAnimator.PlayTargetActionAnimation("Turn_Back", true);
        }
    }

    public float GetAngleOfTarget(Transform characterTransform)
    {
        targetDirection.y = 0;
        float angle = Vector3.Angle(characterTransform.forward, targetDirection);
        Vector3 cross = Vector3.Cross(characterTransform.forward, targetDirection);
        if (cross.y > 0) angle -= angle;
        Debug.Log(angle);
        return angle;
    }
}
