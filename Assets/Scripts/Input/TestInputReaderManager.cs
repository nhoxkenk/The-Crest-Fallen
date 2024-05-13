using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GridBrushBase;

public class TestInputReaderManager : CharacterManager
{
    [SerializeField] private ScriptableInputReader inputReader;

    protected override void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {

        Vector3 movement = new Vector3(inputReader.MoveDirection.x, 0, inputReader.MoveDirection.y);
        Vector3 movementDirection = CameraDirection(movement, false);

        if (inputReader.MovementAmount > 0.5f)
        {
            characterController.Move(movementDirection * 5 * Time.deltaTime);
        }
        else if (inputReader.MovementAmount <= 0.5f)
        {
            characterController.Move(movementDirection * 2 * Time.deltaTime);
        }

        characterAnimator.UpdateAnimatorMovementParameters(0, inputReader.MovementAmount, false);

        Vector3 rotation = new Vector3(inputReader.MoveDirection.x, 0, inputReader.MoveDirection.y);
        Vector3 rotationDirection = CameraDirection(rotation, true);

        //if the player stop moving, all vertical and horizontal value equal to 0
        if (rotationDirection == Vector3.zero)
        {
            rotationDirection = transform.forward;
        }

        Quaternion newRotation = Quaternion.LookRotation(rotationDirection);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, 15f * Time.deltaTime);
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
}
