using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputReader : MonoBehaviour
{
    private PlayerControls playerControls;

    [Header("Player Movement Input")]
    [SerializeField] private Vector2 movementInput;
    public float VerticalInput { get; private set; }
    public float HorizontalInput { get; private set; }
    public float MoveAmount { get; private set; }

    [Header("Camera Movement Input")]
    [SerializeField] private Vector2 cameraInput;
    public float cameraVerticalInput { get; private set; }
    public float cameraHorizontalInput { get; private set; }

    [Header("Player Actions Input")]
    [SerializeField] private bool dodgeInput;
    [SerializeField] private bool sprintInput;
    [SerializeField] private bool jumpInput;
    [SerializeField] private bool leftMouseInput;
    [SerializeField] private bool rightMouseInput;

    [Header("Lock On Input")]
    [SerializeField] private bool LockOnInput;

    private void OnEnable()
    {
        if(playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.Player.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.Camera.Look.performed += i => cameraInput = i.ReadValue<Vector2>();
            playerControls.Player.Dodge.performed += i => dodgeInput = true;
            playerControls.Player.Sprint.performed += i => sprintInput = true;
            playerControls.Player.Sprint.canceled += i => sprintInput = false;
            playerControls.Player.Jump.performed += i => jumpInput = true;

            playerControls.Player.LightAttack.performed += i => leftMouseInput = true;
            playerControls.Player.HeavyAttack.performed += i => rightMouseInput = true;

            playerControls.Player.LockOn.performed += i => LockOnInput = true;
        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Update()
    {
        HandleAllInputs();
    }

    private void HandleAllInputs()
    {
        HandlePlayerMovementInput();
        HandleCameraMovementInput();
        HandleDodgeInput();
        HandleSprintingInput();
        HandleJumpInput();
        HandleLeftMouseInput();
        HandleRightMouseInput();
        HandleLockOnInput();
    }

    private void HandleCameraMovementInput()
    {
        cameraVerticalInput = cameraInput.y;
        cameraHorizontalInput = cameraInput.x;
    }

    private void HandlePlayerMovementInput()
    {
        VerticalInput = movementInput.y;
        HorizontalInput = movementInput.x;

        //Return the absolute number
        MoveAmount = Mathf.Clamp01(Mathf.Abs(VerticalInput) + Mathf.Abs(HorizontalInput));

        //clamp the values, so they are 0, 0.5 or 1
        if (MoveAmount > 0f && MoveAmount <= 0.5f)
        {
            MoveAmount = 0.5f;
        }
        else if (MoveAmount > 0.5f && MoveAmount <= 1f)
        {
            MoveAmount = 1;
        }

        //Only increase horizontal value when lock-on be because we strafe around the target
        PlayerManager.Instance.playerAnimator.UpdateAnimatorMovementParameters(0, MoveAmount, PlayerManager.Instance.playerLocomotion.IsSprinting);
    }

    private void HandleDodgeInput()
    {
        if (dodgeInput)
        {
            dodgeInput = false;

            //perform a dodge
            PlayerManager.Instance.playerLocomotion.AttemptToPerformDodge();
        }
    }

    private void HandleSprintingInput()
    {
        if (sprintInput)
        {
            PlayerManager.Instance.playerLocomotion.HandleSprinting();
        }
        else
        {
            PlayerManager.Instance.playerLocomotion.IsSprinting = false;
        }
    }

    private void HandleJumpInput()
    {
        if (jumpInput)
        {
            jumpInput = false;

            PlayerManager.Instance.playerLocomotion.AttemptToPerformJump();
        }
    }

    private void HandleLeftMouseInput()
    {
        var inventory = PlayerManager.Instance.playerInventory;
        if (leftMouseInput)
        {
            leftMouseInput = false;

            PlayerManager.Instance.playerCombat.SetCharacterActionHand(true);

            PlayerManager.Instance.playerCombat.PerformWeaponBasedAction(inventory.currentRightHandWeapon.leftMouseButtonAction, inventory.currentRightHandWeapon);
        }
    }

    private void HandleRightMouseInput()
    {
        if (rightMouseInput)
        {
            rightMouseInput = false;
        }
    }

    private void HandleLockOnInput()
    {
        if(PlayerManager.Instance.isLockOn)
        {
            if(PlayerManager.Instance.playerCombat.currentTargetManager == null)
            {
                return;
            }

            if(!PlayerManager.Instance.playerCombat.currentTargetManager.IsAlive)
            {
                PlayerManager.Instance.isLockOn = false;
            }
        }

        if (LockOnInput && PlayerManager.Instance.isLockOn)
        {
            LockOnInput = false;
            return;
        }

        if (LockOnInput && !PlayerManager.Instance.isLockOn)
        {
            LockOnInput = false;
            PlayerCamera.Instance.HandleLocatingTargetBeingLockOn();
        }
    }
}
