using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera Instance;

    [HideInInspector] public Camera cameraPlayer;
    [SerializeField] private Transform cameraPivotTransform;

    [Header("Camera Settings")]
    [SerializeField] private float cameraSmoothSpeed = 1;
    [SerializeField] private float horizontalRotationSpeed = 220;
    [SerializeField] private float verticalRotationSpeed = 220;
    [SerializeField] private float miniumPivot = -30;
    [SerializeField] private float maximumPivot = 60;
    [SerializeField] private float cameraCollisionRadius = 0.2f;
    [SerializeField] private LayerMask collideLayer;

    [Header("Camera Values")]
    [SerializeField] private float horizontalLookAngle;
    [SerializeField] private float verticalLookAngle;
    private Vector3 cameraVelocity;
    private Vector3 cameraObjectPositionWhenCollided;
    private float cameraZPosition;
    private float targetCameraZPosition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        cameraPlayer = GetComponentInChildren<Camera>();
    }

    private void Start()
    {
        cameraZPosition = cameraPlayer.transform.localPosition.z;
    }

    public void HandleAllCameraActions()
    {
        if (PlayerManager.Instance == null)
            return;
        HandleFollowTarget();
        HandleRotations();
        HandleCollision();
    }

    private void HandleFollowTarget()
    {
        Vector3 cameraTargetPosition = Vector3.SmoothDamp(transform.position, PlayerManager.Instance.transform.position, ref cameraVelocity, cameraSmoothSpeed * Time.deltaTime);
        transform.position = cameraTargetPosition;
    }

    private void HandleRotations()
    {
        horizontalLookAngle += PlayerManager.Instance.playerInput.cameraHorizontalInput * horizontalRotationSpeed * Time.deltaTime;
        verticalLookAngle -= PlayerManager.Instance.playerInput.cameraVerticalInput * verticalRotationSpeed * Time.deltaTime;
        verticalLookAngle = Mathf.Clamp(verticalLookAngle, miniumPivot, maximumPivot);

        // Left and Right
        Quaternion horizontalTargetRotation = Quaternion.Euler(0f, horizontalLookAngle, 0f);
        transform.rotation = Quaternion.Lerp(transform.rotation, horizontalTargetRotation, horizontalRotationSpeed * Time.deltaTime);

        // Up and Down
        Quaternion verticalTargetRotation = Quaternion.Euler(verticalLookAngle, 0f, 0f);
        cameraPivotTransform.localRotation = Quaternion.Lerp(cameraPivotTransform.localRotation, verticalTargetRotation, verticalRotationSpeed * Time.deltaTime);

    }

    private void HandleCollision()
    {
        targetCameraZPosition = cameraZPosition;
        RaycastHit hit;

        //Direction for collision check
        Vector3 direction = cameraPlayer.transform.position - cameraPivotTransform.position;
        direction.Normalize();

        //We check if there is an object in front of out desired direction
        if (Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetCameraZPosition), collideLayer))
        {
            //if there is, we get distance from it
            float distanceFromHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
            targetCameraZPosition = -(distanceFromHitObject - cameraCollisionRadius);
        }

        //if our target position is less than our collision radius, we subtract out collision radius
        if (Mathf.Abs(targetCameraZPosition) < cameraCollisionRadius)
        {
            targetCameraZPosition = -cameraCollisionRadius;
        }

        //Apply our final position
        cameraObjectPositionWhenCollided.z = Mathf.Lerp(cameraPlayer.transform.localPosition.z, targetCameraZPosition, 0.15f);
        cameraPlayer.transform.localPosition = cameraObjectPositionWhenCollided;
    }
}
