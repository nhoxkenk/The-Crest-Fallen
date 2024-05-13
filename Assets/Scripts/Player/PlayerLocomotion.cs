using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLocomotion : CharacterLocomotion
{
    [HideInInspector] public float verticalMovementValue;
    [HideInInspector] public float horizontalMovementValue;
    [HideInInspector] public float movementAmount;

    private Vector3 movementDirection;
    private Vector3 rotationDirection;

    [Header("Movement Settings")]
    [SerializeField] private float walkingSpeed = 2;
    [SerializeField] private float runningSpeed = 5;
    [SerializeField] private float rotationSpeed = 15f;

    [Header("Jump Setting")]
    [SerializeField] private float jumpHeight = 4;
    [SerializeField] private float jumpFowardVelocity = 5;
    [SerializeField] private float jumpFreeVelocity = 2.5f;
    public int jumpingStaminaCost = 15;
    private Vector3 jumpDirection;

    [Header("Dodge Settings")]
    public int dodgingStaminaCost = 25;
    private Vector3 dodgeDirection;

    [Header("Sprint Settings")]
    [SerializeField] private float sprintSpeed = 8f;
    public int sprintingStaminaCost = 5;

    public bool IsSprinting { get; set; }

    [SerializeField] private ScriptableInputReader inputReader;

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        inputReader.Dodge += OnDodge;
        inputReader.Jump += OnJump;
    }

    private void OnJump(bool jumpInput)
    {
        if (jumpInput)
        {
            AttemptToPerformJump();
        }
    }

    private void OnDodge(bool dodgeInput)
    {
        if (dodgeInput)
        {
            AttemptToPerformDodge();
        }
    }

    public void HandleAllMovement()
    {
        HandleGroundedMovement();
        HandleRotation();
        HandleJumpingMovement();
        HandleFreeFallMovement();

        //New
        HandleSprintInput();
    }

    private void HandleSprintInput()
    {
        if (inputReader.IsSprinting)
        {
            HandleSprinting();
        }
        else
        {
            IsSprinting = false;
        }
    }

    private void HandleGroundedMovement()
    {
        if (!PlayerManager.Instance.canMove)
        {
            return;
        }

        Vector3 movement = new Vector3(inputReader.MoveDirection.x, 0, inputReader.MoveDirection.y);
        movementDirection = CameraDirection(movement, false);

        if (IsSprinting)
        {
            PlayerManager.Instance.characterController.Move(movementDirection * sprintSpeed * Time.deltaTime);
        }
        else
        {
            if (inputReader.MovementAmount > 0.5f)
            {
                PlayerManager.Instance.characterController.Move(movementDirection * runningSpeed * Time.deltaTime);
            }
            else
            if (inputReader.MovementAmount <= 0.5f)
            {
                PlayerManager.Instance.characterController.Move(movementDirection * walkingSpeed * Time.deltaTime);
            }
        }     
        
    }

    private void HandleRotation()
    {
        if (!PlayerManager.Instance.canRotate)
        {
            return;
        }

        Vector3 rotation = new Vector3(inputReader.MoveDirection.x, 0, inputReader.MoveDirection.y);
        rotationDirection = CameraDirection(rotation, true);

        //if the player stop moving, all vertical and horizontal value equal to 0
        if (rotationDirection == Vector3.zero)
        {
            rotationDirection = transform.forward;
        }

        Quaternion newRotation = Quaternion.LookRotation(rotationDirection);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
        transform.rotation = targetRotation;
    }

    public void AttemptToPerformDodge()
    {
        if (PlayerManager.Instance.isPerformingAction)
        {
            return;
        }

        if (PlayerManager.Instance.playerStat.CurrentStamina <= 0)
        {
            return;
        }

        if(inputReader.MovementAmount > 0)
        {
           // Vector3 direction = new Vector3(horizontalMovementValue, 0, verticalMovementValue);
            Vector3 direction = new Vector3(inputReader.MoveDirection.x, 0, inputReader.MoveDirection.y);
            dodgeDirection = CameraDirection(direction, true);

            Quaternion dodgeRotation = Quaternion.LookRotation(dodgeDirection);
            PlayerManager.Instance.transform.rotation = dodgeRotation;
            //Perform Dodge
            PlayerManager.Instance.playerAnimator.PlayTargetActionAnimation("Dodge_To_Idle", true);
        }
        else
        {
            //Perform Back-Step
            PlayerManager.Instance.playerAnimator.PlayTargetActionAnimation("Back_step", true);
        }

        PlayerManager.Instance.playerStat.CurrentStamina -= dodgingStaminaCost;
    }

    public void HandleSprinting()
    {
        if (PlayerManager.Instance.isPerformingAction)
        {
            IsSprinting = false;
            return;
        }

        if(PlayerManager.Instance.playerStat.CurrentStamina <= 0)
        {
            IsSprinting = false;
            return;
        }

        if (inputReader.MovementAmount > 0.5f)
        {
            IsSprinting = true;
        }
        else
        {
            IsSprinting = false;
        }

        if (IsSprinting)
        {
            PlayerManager.Instance.playerStat.CurrentStamina -= sprintingStaminaCost * Time.deltaTime;
        }
    }

    private void HandleFreeFallMovement()
    {
        if(!PlayerManager.Instance.isGrounded)
        {
            Vector3 freeFallDirection = new Vector3(inputReader.MoveDirection.x, 0, inputReader.MoveDirection.y);
            freeFallDirection = CameraDirection(freeFallDirection, true);

            PlayerManager.Instance.characterController.Move(freeFallDirection * jumpFreeVelocity * Time.deltaTime);
        }
    }

    public void AttemptToPerformJump()
    {
        if (PlayerManager.Instance.isPerformingAction)
        {
            return;
        }

        if (PlayerManager.Instance.playerStat.CurrentStamina <= 0)
        {
            return;
        }

        if (PlayerManager.Instance.isJumping)
        {
            return;
        }

        if (!PlayerManager.Instance.isGrounded)
        {
            return;
        }

        PlayerManager.Instance.playerAnimator.PlayTargetActionAnimation("Main_Jump_01", false);
        PlayerManager.Instance.isJumping = true;

        Vector3 direction = new Vector3(inputReader.MoveDirection.x, 0, inputReader.MoveDirection.y);

        jumpDirection = CameraDirection(direction, true);
        PlayerManager.Instance.playerStat.CurrentStamina -= jumpingStaminaCost;

        if (jumpDirection == Vector3.zero)
        {
            return;
        }

        if(IsSprinting)
        {
            jumpDirection *= 1;
        }else if(movementAmount > 0.5)
        {
            jumpDirection *= 0.5f;

        }
        else
        {
            jumpDirection *= 0.25f;
        }
    }

    public void HandleJumpingMovement()
    {
        if (PlayerManager.Instance.isJumping)
        {
            PlayerManager.Instance.characterController.Move(jumpDirection * jumpFowardVelocity * Time.deltaTime);
        }
    }

    //This function add to animation event
    public void ApplyJumpVelocity()
    {
        yVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravityForce);
    }

    private Vector3 CameraDirection(Vector3 movementDirection, bool action)
    {
        Vector3 cameraFoward;
        Vector3 cameraRight;
        if (action)
        {
            cameraFoward = PlayerCamera.Instance.cameraPlayer.transform.forward;
            cameraRight = PlayerCamera.Instance.cameraPlayer.transform.right;
        }
        else
        {
            cameraFoward = PlayerCamera.Instance.transform.forward;
            cameraRight = PlayerCamera.Instance.transform.right;
        }

        cameraFoward.y = 0;
        cameraRight.y = 0;

        return cameraFoward * movementDirection.z + cameraRight * movementDirection.x;
    }
}
