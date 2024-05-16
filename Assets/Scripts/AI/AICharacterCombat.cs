using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacterCombat : CharacterCombat
{
    [Header("Detection Range")]
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
                    characterFinding.characterCombat.SetTarget(targetCharacter);
                }
            }
        }
    }
}
