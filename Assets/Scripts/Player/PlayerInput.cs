using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerControls playerControls;
    private PlayerManager playerManager;

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

    [Header("Player Attacks")]
    [SerializeField] private bool lightAttackInput;
    [SerializeField] private bool heavyAttackInput;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
    }

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

            playerControls.Player.LightAttack.performed += i => lightAttackInput = true;
            playerControls.Player.HeavyAttack.performed += i => heavyAttackInput = true;
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

        HandleAttackInput();
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
        playerManager.playerAnimator.UpdateAnimatorMovementParameters(0, MoveAmount, playerManager.playerLocomotion.IsSprinting);
    }

    private void HandleDodgeInput()
    {
        if (dodgeInput)
        {
            dodgeInput = false;

            //perform a dodge
            playerManager.playerLocomotion.AttemptToPerformDodge();
        }
    }

    private void HandleSprintingInput()
    {
        if (sprintInput)
        {
            playerManager.playerLocomotion.HandleSprinting();
        }
        else
        {
            playerManager.playerLocomotion.IsSprinting = false;
        }
    }

    private void HandleJumpInput()
    {
        if (jumpInput)
        {
            jumpInput = false;

            playerManager.playerLocomotion.AttemptToPerformJump();
        }
    }

    private void HandleAttackInput()
    {
        if (lightAttackInput)
        {
            lightAttackInput = false;
        }

        if (heavyAttackInput)
        {
            heavyAttackInput = false;
        }
    }
}
