using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput), typeof(PlayerLocomotion))]
public class PlayerManager : CharacterManager
{
    [HideInInspector] public PlayerInput playerInput;
    [HideInInspector] public PlayerLocomotion playerLocomotion;
    [HideInInspector] public PlayerAnimator playerAnimator;

    protected override void Awake()
    {
        base.Awake();
        playerInput = GetComponent<PlayerInput>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerAnimator = GetComponentInChildren<PlayerAnimator>();
    }

    private void Start()
    {

    }

    private void Update()
    {
        playerLocomotion.HandleAllMovement();
    }

    private void LateUpdate()
    {
        PlayerCamera.Instance.HandleAllCameraActions();
    }
}
