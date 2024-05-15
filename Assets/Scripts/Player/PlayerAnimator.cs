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
        if (MoveAmount > 0f && MoveAmount <= 0.5f)
        {
            MoveAmount = 0.5f;
        }
        else if (MoveAmount > 0.5f && MoveAmount <= 1f)
        {
            MoveAmount = 1;
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
        if (PlayerManager.Instance.applyRootMotion)
        {
            Vector3 velocity = PlayerManager.Instance.animator.deltaPosition;
            PlayerManager.Instance.characterController.Move(velocity);
            PlayerManager.Instance.transform.rotation *= PlayerManager.Instance.animator.deltaRotation;
        }
    }
}
