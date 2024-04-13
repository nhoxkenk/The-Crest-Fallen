using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : CharacterAnimator
{
    protected override void Awake()
    {
        base.Awake();
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
