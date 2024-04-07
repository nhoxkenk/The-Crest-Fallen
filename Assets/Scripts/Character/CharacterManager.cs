using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [HideInInspector] public CharacterController characterController;
    [HideInInspector] public Animator animator;
    [HideInInspector] public CharacterAnimator characterAnimator;
    [HideInInspector] public CharacterLocomotion characterLocomotion;


   [Header("Flags")]
    public bool isPerformingAction = false;
    public bool applyRootMotion = false;
    public bool canMove = true;
    public bool canRotate = true;
    public bool isJumping = false;
    public bool isGrounded = true;

    protected virtual void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        characterAnimator = GetComponentInChildren<CharacterAnimator>();
        characterLocomotion = GetComponent<CharacterLocomotion>();
    }

    protected virtual void Update()
    {
        animator.SetBool(characterAnimator.isGroundedValue, isGrounded);
    }
}
