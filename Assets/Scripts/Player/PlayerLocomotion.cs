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
        if (!PlayerManager.Instance.CanMove)
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
            if (inputReader.MovementAmount > 0.5f && !PlayerManager.Instance.SwitchToWalking)
            {
                PlayerManager.Instance.characterController.Move(movementDirection * runningSpeed * Time.deltaTime);
            }
            else
            if (inputReader.MovementAmount <= 0.5f || PlayerManager.Instance.SwitchToWalking)
            {
                PlayerManager.Instance.characterController.Move(movementDirection * walkingSpeed * Time.deltaTime);
            }
        }     
        
    }

    private void HandleRotation()
    {
        if (!PlayerManager.Instance.CanRotate)
        {
            return;
        }

        if (PlayerManager.Instance.IsLockOn)
        {
            if (IsSprinting)
            {
                Vector3 targetDirection = new Vector3(inputReader.MoveDirection.x, 0, inputReader.MoveDirection.y);
                targetDirection = CameraDirection(targetDirection, true);
                targetDirection.Normalize();

                if(targetDirection == Vector3.zero)
                {
                    targetDirection = transform.forward;
                }

                Quaternion rotation = Quaternion.LookRotation(targetDirection);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
            }
            else
            {
                var target = PlayerManager.Instance.playerCombat.currentTarget;
                if (target == null)
                {
                    return;
                }

                Vector3 direction = target.transform.position - transform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = transform.forward;
                }

                Quaternion rotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
            }
        }
        else
        {
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
    }

    public void AttemptToPerformDodge()
    {
        if (PlayerManager.Instance.IsPerformingAction || PlayerManager.Instance.IsJumping)
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

        PlayerManager.Instance.characterSoundEffect.PlaySoundFX(SoundEffectsManager.Instance.rollSFX);
        PlayerManager.Instance.playerStat.CurrentStamina -= dodgingStaminaCost;
    }

    public void HandleSprinting()
    {
        if (PlayerManager.Instance.IsPerformingAction)
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
        if(!PlayerManager.Instance.IsGrounded)
        {
            Vector3 freeFallDirection = new Vector3(inputReader.MoveDirection.x, 0, inputReader.MoveDirection.y);
            freeFallDirection = CameraDirection(freeFallDirection, true);

            PlayerManager.Instance.characterController.Move(freeFallDirection * jumpFreeVelocity * Time.deltaTime);
        }
    }

    public void AttemptToPerformJump()
    {
        if (PlayerManager.Instance.IsPerformingAction)
        {
            return;
        }

        if (PlayerManager.Instance.playerStat.CurrentStamina <= 0)
        {
            return;
        }

        if (PlayerManager.Instance.IsJumping)
        {
            return;
        }

        if (!PlayerManager.Instance.IsGrounded)
        {
            return;
        }

        PlayerManager.Instance.playerAnimator.PlayTargetActionAnimation("Main_Jump_01", false);
        PlayerManager.Instance.IsJumping = true;

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
        if (PlayerManager.Instance.IsJumping)
        {
            PlayerManager.Instance.characterController.Move(jumpDirection * jumpFowardVelocity * Time.deltaTime);
        }
    }

    //This function add to animation event
    public void ApplyJumpVelocity()
    {
        yVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravityForce);
    }

    public void SwitchToRunning()
    {
        PlayerManager.Instance.SwitchToWalking = false;
    }

    public void SwitchToWalking()
    {
        PlayerManager.Instance.SwitchToWalking = true;
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
