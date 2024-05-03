using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
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
}
