using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    private CharacterManager characterManager;
    private readonly int HorizontalValue = Animator.StringToHash("Horizontal");
    private readonly int VericalValue = Animator.StringToHash("Vertical");
    public readonly int isGroundedValue = Animator.StringToHash("isGrounded");
    public readonly int inAirTimerValue = Animator.StringToHash("inAirTimer");

    protected virtual void Awake()
    {
        characterManager = GetComponentInParent<CharacterManager>();
    }

    public void UpdateAnimatorMovementParameters(float horizontalValue, float verticalValue, bool isSprinting)
    {
        float horizontal = horizontalValue;
        float vertical = verticalValue;
        if (isSprinting)
        {
            vertical = 2;
        }
        characterManager.animator.SetFloat(HorizontalValue, horizontal, 0.1f, Time.deltaTime);
        characterManager.animator.SetFloat(VericalValue, vertical, 0.1f, Time.deltaTime);
    }

    //Whenever we need a specific animation, this function is call, ex: attack, dodge, heal, ...
    public virtual void PlayTargetActionAnimation(string targetAnimationName, bool isPerformingAction, bool applyRootMotion = true, bool canMove = false, bool canRotate = false)
    {
        characterManager.applyRootMotion = applyRootMotion;
        characterManager.animator.CrossFade(targetAnimationName, 0.2f);
        //Stop character from attempting a new actions
        characterManager.isPerformingAction = isPerformingAction;
        characterManager.canMove = canMove;
        characterManager.canRotate = canRotate;
    }
}
