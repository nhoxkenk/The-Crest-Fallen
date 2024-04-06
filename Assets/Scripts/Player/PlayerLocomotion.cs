using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLocomotion : MonoBehaviour
{
    private PlayerManager playerManager;

    [HideInInspector] public float verticalMovementValue;
    [HideInInspector] public float horizontalMovementValue;
    [HideInInspector] public float movementAmount;

    private Vector3 movementDirection;
    private Vector3 rotationDirection;

    [Header("Movement Settings")]
    [SerializeField] private float walkingSpeed = 2;
    [SerializeField] private float runningSpeed = 5;
    [SerializeField] private float rotationSpeed = 15f;

    [Header("Dodge Settings")]
    private Vector3 dodgeDirection;

    [Header("Sprint Settings")]
    [SerializeField] private float sprintSpeed = 8f;
    public bool IsSprinting { get; set; }

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
    }

    public void HandleAllMovement()
    {
        GetVerticalAndHorizontalInput();
        HandleGroundedMovement();
        HandleRotation();
    }

    private void GetVerticalAndHorizontalInput()
    {
        verticalMovementValue = playerManager.playerInput.VerticalInput;
        horizontalMovementValue = playerManager.playerInput.HorizontalInput;
        movementAmount = playerManager.playerInput.MoveAmount;
    }

    private void HandleGroundedMovement()
    {
        if (!playerManager.canMove)
        {
            return;
        }

        Vector3 movement = new Vector3(horizontalMovementValue, 0, verticalMovementValue);
        movementDirection = CameraDirection(movement, false);

        if (IsSprinting)
        {
            playerManager.characterController.Move(movementDirection * sprintSpeed * Time.deltaTime);
        }
        else
        {
            if (movementAmount > 0.5f)
            {
                playerManager.characterController.Move(movementDirection * runningSpeed * Time.deltaTime);
            }
            else
            if (movementAmount <= 0.5f)
            {
                playerManager.characterController.Move(movementDirection * walkingSpeed * Time.deltaTime);
            }
        }     
        
    }

    private void HandleRotation()
    {
        if (!playerManager.canRotate)
        {
            return;
        }

        Vector3 rotation = new Vector3(horizontalMovementValue, 0, verticalMovementValue);
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

    public void AttemptToPerformDodge()
    {
        if (playerManager.isPerformingAction)
        {
            return;
        }
        if (movementAmount > 0)
        {
            Vector3 direction = new Vector3(horizontalMovementValue, 0, verticalMovementValue);
            dodgeDirection = CameraDirection(direction, true);

            Quaternion dodgeRotation = Quaternion.LookRotation(dodgeDirection);
            playerManager.transform.rotation = dodgeRotation;
            //Perform Dodge
            playerManager.playerAnimator.PlayTargetActionAnimation("Dodge_To_Idle", true);
        }
        else
        {
            //Perform Back-Step
            playerManager.playerAnimator.PlayTargetActionAnimation("Back_step", true);
        }
    }

    public void HandleSprinting()
    {
        if (playerManager.isPerformingAction)
        {
            IsSprinting = false;
            return;
        }

        if (movementAmount > 0.5f)
        {
            IsSprinting = true;
        }
        else
        {
            IsSprinting = false;
        }

    }
}
