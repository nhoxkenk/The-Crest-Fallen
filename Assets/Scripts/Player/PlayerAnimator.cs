using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : CharacterAnimator
{
    private PlayerManager playerManager;

    protected override void Awake()
    {
        base.Awake();
        playerManager = GetComponentInParent<PlayerManager>();
    }

    private void OnAnimatorMove()
    {
        if (playerManager.applyRootMotion)
        {
            Vector3 velocity = playerManager.animator.deltaPosition;
            playerManager.characterController.Move(velocity);
            playerManager.transform.rotation *= playerManager.animator.deltaRotation;
        }
    }
}
