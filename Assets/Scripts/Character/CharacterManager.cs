using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [HideInInspector] public CharacterController characterController;
    [HideInInspector] public Animator characterAnimator;

    [Header("Flags")]
    public bool isPerformingAction = false;
    public bool applyRootMotion = false;
    public bool canMove = true;
    public bool canRotate = true;

    protected virtual void Awake()
    {
        characterController = GetComponent<CharacterController>();
        characterAnimator = GetComponentInChildren<Animator>();
    }
}
