using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : CharacterAnimator
{
    [SerializeField] private ScriptableInputReader inputReader;

    protected override void Awake()
    {
        base.Awake();
    }

    public void UpdateAnimator()
    {
        float MoveAmount = inputReader.MovementAmount;

        PlayerManager.Instance.IsMoving = MoveAmount != 0 ? true : false;

        if (MoveAmount > 0f && MoveAmount <= 0.5f)
        {
            MoveAmount = 0.5f;
        }
        else if (MoveAmount > 0.5f && MoveAmount <= 1f)
        {
            MoveAmount = 1;
        }

        if(PlayerManager.Instance.SwitchToWalking)
        {
            MoveAmount = 0.45f;
        }

        if(PlayerManager.Instance.IsLockOn && !PlayerManager.Instance.playerLocomotion.IsSprinting)
        {
            UpdateAnimatorMovementParameters(inputReader.MoveDirection.x, inputReader.MoveDirection.y, PlayerManager.Instance.playerLocomotion.IsSprinting);
        }
        else
        {
            UpdateAnimatorMovementParameters(0, MoveAmount, PlayerManager.Instance.playerLocomotion.IsSprinting);
        }
    }

    private void OnAnimatorMove()
    {
        if (PlayerManager.Instance.ApplyRootMotion)
        {
            Vector3 velocity = PlayerManager.Instance.animator.deltaPosition;
            PlayerManager.Instance.characterController.Move(velocity);
            PlayerManager.Instance.transform.rotation *= PlayerManager.Instance.animator.deltaRotation;
        }
    }

    /// <summary>
    /// This function is register as an event in Animation events of Light Attack Animation.
    /// </summary>
    public override void EnableCanDoCombo()
    {
        if (PlayerManager.Instance.playerCombat.IsUsingRightHandWeapon)
        {
            PlayerManager.Instance.playerCombat.canComboWithMainHandWeapon = true;
        }
    }

    /// <summary>
    /// This function is register as an event in Animation events of Light Attack Animation.
    /// </summary>
    public override void DisableCanDoCombo()
    {
        PlayerManager.Instance.playerCombat.canComboWithMainHandWeapon = false;
    }
}
