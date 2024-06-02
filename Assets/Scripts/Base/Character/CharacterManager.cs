using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [HideInInspector] public CharacterController characterController;
    [HideInInspector] public Animator animator;
    [HideInInspector] public CharacterAnimator characterAnimator;
    [HideInInspector] public CharacterLocomotion characterLocomotion;
    [HideInInspector] public CharacterStat characterStat;
    [HideInInspector] public CharacterEffects characterEffects;
    [HideInInspector] public CharacterInventory characterInventory;
    [HideInInspector] public CharacterEquipment characterEquipment;
    [HideInInspector] public CharacterCombat characterCombat;
    [HideInInspector] public CharacterSoundEffect characterSoundEffect;

    [Header("Character Type")]
    public CharacterType type;
    public CharacterType Type {  get => type; private set => type = value; }

    [Header("Status")]
    [SerializeField] private bool isAlive = true;
    [SerializeField] private bool isMoving = false;
    public bool IsAlive { get => isAlive; set => isAlive = value; }
    public bool IsMoving 
    {   get => isMoving; 
        set 
        {
            isMoving = value;
            animator.SetBool(characterAnimator.isMoving, isMoving);
        }  
    }

    [Header("Animation Flags")]
    [SerializeField] private bool isPerformingAction = false;
    [SerializeField] private bool applyRootMotion = false;

    public bool IsPerformingAction
    {
        get => isPerformingAction;
        set => isPerformingAction = value;
    }

    public bool ApplyRootMotion
    {
        get => applyRootMotion;
        set => applyRootMotion = value;
    }

    [Header("Movement Flags")]
    [SerializeField] private bool canMove = true;
    [SerializeField] private bool canRotate = true;

    public bool CanMove
    {
        get => canMove;
        set => canMove = value;
    }

    public bool CanRotate
    {
        get => canRotate;
        set => canRotate = value;
    }

    [Header("Jump Flags")]
    [SerializeField] private bool isJumping = false;
    [SerializeField] private bool isGrounded = true;

    public bool IsJumping
    {
        get => isJumping;
        set => isJumping = value;
    }

    public bool IsGrounded
    {
        get => isGrounded;
        set => isGrounded = value;
    }

    protected virtual void Awake()
    {
        DontDestroyOnLoad(this);
        GetComponents();
    }

    protected virtual void Start()
    {
        IgnoreCharacterOwnCollision();
    }

    protected virtual void Update()
    {
        animator.SetBool(characterAnimator.isGroundedValue, IsGrounded);
    }

    protected virtual void FixedUpdate()
    {
        
    }

    protected virtual void GetComponents()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        characterAnimator = GetComponentInChildren<CharacterAnimator>();
        characterLocomotion = GetComponent<CharacterLocomotion>();
        characterStat = GetComponent<CharacterStat>();
        characterEffects = GetComponent<CharacterEffects>();
        characterInventory = GetComponent<CharacterInventory>();
        characterEquipment = GetComponent<CharacterEquipment>();
        characterCombat = GetComponent<CharacterCombat>();
        characterSoundEffect = GetComponent<CharacterSoundEffect>();
    }

    public virtual IEnumerator ProcessDeathEvent(bool manualSelectDeathAnimation = false)
    {
        if (!manualSelectDeathAnimation)
        {
            characterAnimator.PlayTargetActionAnimation("Death_01", true);
        }

        isAlive = false;
        characterStat.CurrentHealth = 0;

        yield return new WaitForSeconds(5f);    
    }

    public virtual void ReviveCharacter() { }

    private void IgnoreCharacterOwnCollision()
    {
        Collider characterControllerCollider = GetComponent<Collider>();
        Collider[] characterDamageableCollider = GetComponentsInChildren<Collider>();

        List<Collider> collidersWillBeIgnore = new List<Collider>();

        foreach(var collider in characterDamageableCollider)
        {
            collidersWillBeIgnore.Add(collider);
        }
        collidersWillBeIgnore.Add(characterControllerCollider);

        foreach(var collider in collidersWillBeIgnore)
        {
            foreach (var otherCollider in collidersWillBeIgnore)
            {
                Physics.IgnoreCollision(collider, otherCollider, true);
            }
        }  
    }

    public bool CanDealDamageTo(CharacterManager characterManager)
    {
        return Type != characterManager.Type;
    }
}
