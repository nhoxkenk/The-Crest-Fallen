using System;
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

    [Header("Lock On")]
    [SerializeField] private float lockOnRadius = 20;
    [SerializeField] private float viewableAngle = 50;
    [SerializeField] private float lockOnRotateSpeed = 5;
    [SerializeField] private List<CharacterManager> potentialTarget = new List<CharacterManager>();
    [SerializeField] private CharacterManager nearestLockOnTarget;

    //New change
    [SerializeField] private ScriptableInputReader inputReader;

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

    private void OnEnable()
    {
        inputReader.LockOn += OnLockOn;
    }

    private void OnLockOn(bool lockOnInput)
    {
        if (PlayerManager.Instance.IsLockOn)
        {
            if (PlayerManager.Instance.playerCombat.currentTarget == null)
            {
                return;
            }

            if (!PlayerManager.Instance.playerCombat.currentTarget.IsAlive)
            {
                ClearLockOnTargets();
            }
        }

        if (lockOnInput && PlayerManager.Instance.IsLockOn)
        {
            ClearLockOnTargets();
            return;
        }

        if (lockOnInput && !PlayerManager.Instance.IsLockOn)
        {
            HandleLocatingTargetBeingLockOn();
            if(nearestLockOnTarget != null)
            {
                PlayerManager.Instance.IsLockOn = true;
                PlayerManager.Instance.playerCombat.SetTarget(nearestLockOnTarget);
            }
        }
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
        if (PlayerManager.Instance.IsLockOn)
        {
            //Rotate this gameObject (Camera)
            Vector3 direction = (PlayerManager.Instance.playerCombat.currentTarget.characterCombat.lockOnTransform.position - transform.position).normalized;
            direction.y = 0;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, lockOnRotateSpeed);

            //Rotate the gameObject pivot (Camera Pivot)
            direction = (PlayerManager.Instance.playerCombat.currentTarget.characterCombat.lockOnTransform.position - cameraPivotTransform.position).normalized;

            rotation = Quaternion.LookRotation(direction);
            cameraPivotTransform.transform.rotation = Quaternion.Lerp(cameraPivotTransform.transform.rotation, rotation, lockOnRotateSpeed);

            //Save the rotation for when we unlock the lock on
            horizontalLookAngle = transform.eulerAngles.y;
            verticalLookAngle = transform.eulerAngles.x;
        }
        else
        {
            horizontalLookAngle += inputReader.LookDirection.x * horizontalRotationSpeed * Time.deltaTime;
            verticalLookAngle -= inputReader.LookDirection.y * verticalRotationSpeed * Time.deltaTime;
            verticalLookAngle = Mathf.Clamp(verticalLookAngle, miniumPivot, maximumPivot);

            // Left and Right
            Quaternion horizontalTargetRotation = Quaternion.Euler(0f, horizontalLookAngle, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, horizontalTargetRotation, horizontalRotationSpeed * Time.deltaTime);

            // Up and Down
            Quaternion verticalTargetRotation = Quaternion.Euler(verticalLookAngle, 0f, 0f);
            cameraPivotTransform.localRotation = Quaternion.Lerp(cameraPivotTransform.localRotation, verticalTargetRotation, verticalRotationSpeed * Time.deltaTime);
        }
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

    public void HandleLocatingTargetBeingLockOn()
    {
        float shortestDistance = Mathf.Infinity;
        //float shortestDistanceOfRightTarget = Mathf.Infinity;       //Shortest Distance on one axis to the right of the current target (+)
        //float shortestDistanceOfleftTarget = -Mathf.Infinity;       //Shortest Distance on one axis to the left of the current target (-)

        Collider[] colliders = Physics.OverlapSphere(PlayerManager.Instance.transform.position, lockOnRadius, CharacterLayersManager.Instance.CharacterLayerMask);

        for(int i = 0; i < colliders.Length; i++)
        {
            CharacterManager lockOnTarget = colliders[i].GetComponent<CharacterManager>();

            if(lockOnTarget != null)
            {
                //if target is within the viewable field
                Vector3 lockOnTargetDirection = Vector3.Normalize(lockOnTarget.transform.position - PlayerManager.Instance.transform.position);
                float distanceFromTarget = Vector3.Distance(PlayerManager.Instance.transform.position, lockOnTarget.transform.position);
                float viewableAngle = Vector3.Angle(lockOnTargetDirection, cameraPlayer.transform.forward);

                //if target is dead
                if(!lockOnTarget.IsAlive)
                {
                    continue;
                }

                //if target is us
                if(lockOnTarget.transform.root == PlayerManager.Instance.transform.root)
                {
                    continue;
                }

                if(viewableAngle > -this.viewableAngle && viewableAngle < this.viewableAngle)
                {
                    RaycastHit hit;

                    if (Physics.Linecast(PlayerManager.Instance.playerCombat.lockOnTransform.position, 
                        lockOnTarget.characterCombat.lockOnTransform.position, 
                        out hit, CharacterLayersManager.Instance.EnvironmentLayerMask))
                    {
                        continue;
                    }
                    else
                    {
                        Debug.Log("We have made it");
                        potentialTarget.Add(lockOnTarget);
                    }
                }
            }
        }

        for(int i = 0; i < potentialTarget.Count; i++)
        {
            if (potentialTarget[i] != null)
            {
                float distance = Vector3.Distance(PlayerManager.Instance.transform.position, potentialTarget[i].transform.position);

                if(distance <= shortestDistance)
                {
                    shortestDistance = distance;
                    nearestLockOnTarget = potentialTarget[i];
                }
            }
            else
            {
                ClearLockOnTargets();
            }
        }
    }

    public void ClearLockOnTargets()
    {
        PlayerManager.Instance.IsLockOn = false;
        nearestLockOnTarget = null;
        potentialTarget.Clear();
    }
}
