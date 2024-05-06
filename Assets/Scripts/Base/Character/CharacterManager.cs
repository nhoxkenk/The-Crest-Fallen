using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterManager : MonoBehaviour, IEffectable
{
    [HideInInspector] public CharacterController characterController;
    [HideInInspector] public Animator animator;
    [HideInInspector] public CharacterAnimator characterAnimator;
    [HideInInspector] public CharacterLocomotion characterLocomotion;
    [HideInInspector] public CharacterStat characterStat;
    [HideInInspector] public CharacterEffects characterEffects;

    [Header("Status")]
    public bool isAlive = true;

   [Header("Animation Flags")]
    public bool isPerformingAction = false;
    public bool applyRootMotion = false;

    [Header("Movement Flags")]
    public bool canMove = true;
    public bool canRotate = true;

    [Header("Jump Flags")]
    public bool isJumping = false;
    public bool isGrounded = true;

    #region Effectable Interface Implement
    public bool IsAlive { get => isAlive; set => isAlive = value; }

    public void ProcessInstantEffects(ScriptableInstantCharacterEffect effect)
    {
        characterEffects.ProcessInstantEffects(effect);
    }

    public virtual void TakeInstantHealthEffect(float damage)
    {
        characterStat.CurrentHealth -= damage;
    }

    public virtual void TakeInstantStaminaEffect(float damage)
    {
        characterStat.CurrentStamina -= damage;
    }
    #endregion

    protected virtual void Awake()
    {
        DontDestroyOnLoad(this);

        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        characterAnimator = GetComponentInChildren<CharacterAnimator>();
        characterLocomotion = GetComponent<CharacterLocomotion>();
        characterStat = GetComponent<CharacterStat>();
        characterEffects = GetComponent<CharacterEffects>();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        animator.SetBool(characterAnimator.isGroundedValue, isGrounded);
    }

    public virtual IEnumerator ProcessDeathEvent(bool manualSelectDeathAnimation = false)
    {
        if (!manualSelectDeathAnimation)
        {
            characterAnimator.PlayTargetActionAnimation("Death_01", true);
        }

        isAlive = false;
        characterStat.CurrentHealth = 0;

        yield return new WaitForSeconds(5f);    
    }

    public virtual void ReviveCharacter() { }
}
