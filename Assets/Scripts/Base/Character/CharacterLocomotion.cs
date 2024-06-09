using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLocomotion : MonoBehaviour
{
    private CharacterManager characterManager;

    [Header("Ground check & Jumping")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 1;
    [SerializeField] protected float groundedVelocity = -20;
    [SerializeField] protected float fallVelocity = -5;
    [SerializeField] protected float gravityForce = -5.55f;
    [SerializeField] protected Vector3 yVelocity;
    protected bool fallingVelocityHasBeenSet = false;
    protected float inAirTimer = 0;

    protected virtual void Awake()
    {
        characterManager = GetComponent<CharacterManager>();
    }

    protected virtual void Update()
    {
        HandleGroundCheck();
        //Check if the player if falling or not ?
        HandleGroundCondition();

        characterManager.characterController.Move(yVelocity * Time.deltaTime);
    }

    protected void HandleGroundCheck()
    {
        characterManager.IsGrounded = Physics.CheckSphere(characterManager.transform.position, groundCheckRadius, groundLayer);
    }

    protected void HandleGroundCondition()
    {
        if (characterManager.IsGrounded)
        {
            //Not attempting to jump or move up
            if (yVelocity.y < 0)
            {
                inAirTimer = 0;
                fallingVelocityHasBeenSet = false;
                yVelocity.y = groundedVelocity;
            }
        }
        else
        {
            //Not jump, just falling
            if (!characterManager.IsJumping && !fallingVelocityHasBeenSet)
            {
                fallingVelocityHasBeenSet = true;
                yVelocity.y = fallVelocity;
            }
            inAirTimer += Time.deltaTime;
            characterManager.animator.SetFloat(characterManager.characterAnimator.inAirTimerValue, inAirTimer);

            yVelocity.y += gravityForce * Time.deltaTime;
        }
    }

    //Register as animation event
    public void EnableCanRotate()
    {
        characterManager.CanRotate = true;
    }

    //Register as animation event
    public void DisableCanRotate()
    {
        characterManager.CanRotate = false;
    }
}
