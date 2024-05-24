using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacterCombat : CharacterCombat
{
    [Header("Attacking Rotate Speed")]
    [SerializeField] private float attackRotatedSpeed = 25f;

    [Header("Target Infomations")]
    public float distanceFromTarget;
    public Vector3 targetDirection;

    [Header("Detection Range")]
    public float characterViewableAngle;

    [SerializeField] private float detectionRadius = 15f;
    [SerializeField] private float miniumDetectionAngle = -25;
    [SerializeField] private float maxiumDetectionAngle = 35;

    public CountdownTimer actionTimer;

    private void Awake()
    {
        actionTimer = new CountdownTimer(2f);
    }

    public override void DrainStaminaBaseOnWeaponAction()
    {
        //noop
    }

    public void FindTargetViaLineOfSight(AICharacterManager characterFinding)
    {
        if (currentTarget != null)
        {
            return;
        }

        Collider[] colliders = Physics.OverlapSphere(characterFinding.transform.position, detectionRadius, CharacterLayersManager.Instance.CharacterLayerMask);

        foreach (Collider collider in colliders)
        {
            CharacterManager targetCharacter = collider.GetComponent<CharacterManager>();

            if (targetCharacter == null)
                continue;

            if (!targetCharacter.IsAlive || targetCharacter == characterFinding)
            {
                continue;
            }

            if (characterFinding.CanDealDamageTo(targetCharacter))
            {
                Vector3 targetDistance = characterFinding.transform.position - targetCharacter.transform.position;
                float viewableAngle = Vector3.Angle(targetDistance, targetCharacter.transform.position);

                if (viewableAngle < miniumDetectionAngle || viewableAngle > maxiumDetectionAngle)
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

        if (characterViewableAngle >= 40 && characterViewableAngle <= 70)
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

    public void RotateTowardsTarget(AICharacterManager character)
    {
        if (character.IsMoving)
        {
            character.transform.rotation = character.agent.transform.rotation;
        }
    }

    public void RotateTowardsTargetWhileAttack(AICharacterManager character)
    {
        if(currentTarget == null || !character.IsPerformingAction || !character.CanRotate)
        {
            return;
        }

        Vector3 targetDirection = currentTarget.transform.position - character.transform.position;
        targetDirection.y = 0;
        targetDirection.Normalize();

        if(targetDirection == Vector3.zero)
        {
            targetDirection = character.transform.forward;  
        }

        Quaternion rotation = Quaternion.LookRotation(targetDirection);
        character.transform.rotation = Quaternion.Lerp(character.transform.rotation, rotation, attackRotatedSpeed);
    }

    public float GetAngleOfTarget(Transform characterTransform)
    {
        targetDirection.y = 0;
        float angle = Vector3.Angle(characterTransform.forward, targetDirection);
        Vector3 cross = Vector3.Cross(characterTransform.forward, targetDirection);
        if (cross.y > 0) angle -= angle;
        return angle;
    }

    public void HandleActionTimer(AICharacterManager aiManager)
    {
        if (!actionTimer.IsFinished() && actionTimer.IsRunning)
        {
            if (!aiManager.IsPerformingAction)
            {
                actionTimer.Tick(Time.deltaTime);
            }
        }
    }
}
