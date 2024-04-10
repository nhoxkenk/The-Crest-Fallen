using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput), typeof(PlayerLocomotion))]
public class PlayerManager : CharacterManager
{
    [HideInInspector] public PlayerInput playerInput;
    [HideInInspector] public PlayerLocomotion playerLocomotion;
    [HideInInspector] public PlayerAnimator playerAnimator;
    [HideInInspector] public PlayerInventory playerInventory;

   protected override void Awake()
    {
        base.Awake();
        playerInput = GetComponent<PlayerInput>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerAnimator = GetComponentInChildren<PlayerAnimator>();
        playerInventory = GetComponent<PlayerInventory>();
    }

    private void Start()
    {

    }

    protected override void Update()
    {
        base.Update();
        playerLocomotion.HandleAllMovement();
    }

    private void LateUpdate()
    {
        PlayerCamera.Instance.HandleAllCameraActions();
    }
}
