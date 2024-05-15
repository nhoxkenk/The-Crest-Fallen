using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    protected CharacterManager characterManager;
    private readonly int HorizontalValue = Animator.StringToHash("Horizontal");
    private readonly int VericalValue = Animator.StringToHash("Vertical");
    public readonly int isGroundedValue = Animator.StringToHash("isGrounded");
    public readonly int inAirTimerValue = Animator.StringToHash("inAirTimer");
    public readonly int isChargingAttack = Animator.StringToHash("isChargingAttack");

    [Header("Damage Animation")]
    public string hit_Forward_Medium_01 = "hit_Forward_Medium_01";
    public string hit_Backward_Medium_01 = "hit_Backward_Medium_01";
    public string hit_Left_Medium_01 = "hit_Left_Medium_01";
    public string hit_Right_Medium_01 = "hit_Right_Medium_01";

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

    public virtual void PlayTargetAttackAnimation(AttackType type, string targetAnimationName, bool isPerformingAction, bool applyRootMotion = true, bool canMove = false, bool canRotate = false)
    {
        //Keep track of last attack
        characterManager.characterCombat.currentAttackType = type;

        characterManager.applyRootMotion = applyRootMotion;
        characterManager.animator.CrossFade(targetAnimationName, 0.2f);
        characterManager.isPerformingAction = isPerformingAction;
        characterManager.canMove = canMove;
        characterManager.canRotate = canRotate;
    }

    public void HandleIsChargingAttack(bool value)
    {
        characterManager.animator.SetBool(isChargingAttack, value);
    }
}
