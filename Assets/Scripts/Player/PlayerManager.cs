using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : CharacterManager
{
    [Header("Lock On Flags")]
    [SerializeField] private bool isLockOn;

    public bool IsLockOn 
    { 
        get => isLockOn; 
        set 
        {
            OnIsLockOnChanged?.Invoke(value);
            isLockOn = value; 
        } 
    }
    private event UnityAction<bool> OnIsLockOnChanged;

    [Header("Debug")]
    public bool revive;
    public bool switchRightWeapon;

    private static PlayerManager instance;
    public static PlayerManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerManager>();
            }
            return instance;
        }
    }

    [HideInInspector] public InputReader playerInput;
    [HideInInspector] public PlayerLocomotion playerLocomotion;
    [HideInInspector] public PlayerAnimator playerAnimator;
    [HideInInspector] public PlayerInventory playerInventory;
    [HideInInspector] public PlayerStat playerStat;
    [HideInInspector] public PlayerSaveData playerSaveData;
    [HideInInspector] public PlayerEquipment playerEquipment;
    [HideInInspector] public PlayerCombat playerCombat;

   protected override void Awake()
    {
        base.Awake();

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        GetComponents();
    }

    protected override void Start()
    {
        base.Start();
        BindingPlayerEvents();
        InitializePlayerStat();
    }

    protected override void Update()
    {
        base.Update();

        if (IsAlive)
        {
            playerLocomotion.HandleAllMovement();

            playerAnimator.UpdateAnimator();

            playerStat.RegenerateStamina();
        }

        DebugMenu();
    }

    private void LateUpdate()
    {
        PlayerCamera.Instance.HandleAllCameraActions();
    }

    /// <summary>
    /// Initialize Player Stat
    /// </summary>
    private void InitializePlayerStat()
    {
        //Stamina
        playerStat.maxStamina = playerStat.CalculateStaninaBasedOnEnduranceLevel(playerSaveData.data.endurance);
        playerStat.CurrentStamina = playerSaveData.data.currentStamina;
        PlayerUI.Instance.playerUIHud.HandleMaxStaminaValue(playerStat.maxStamina);
        playerStat.IncreaseVitalityStat += playerStat.OnIncreaseVitalityStat;

        //Health
        playerStat.maxHealth = playerStat.CalculateHealthBasedOnVitalityLevel(playerSaveData.data.vitality);
        playerStat.CurrentHealth = playerSaveData.data.currentHealth;
        PlayerUI.Instance.playerUIHud.HandleMaxHealthValue(playerStat.maxHealth);
        playerStat.IncreaseEnduranceStat += playerStat.OnIncreaseEnduranceStat;
    }

    private void BindingPlayerEvents()
    {
        //Stats
        playerStat.DrainingStamina += PlayerUI.Instance.playerUIHud.HandleNewStaminaValue;
        playerStat.RegeneratingStamina += PlayerUI.Instance.playerUIHud.HandleNewStaminaValue;

        playerStat.CurrentHealthChange += PlayerUI.Instance.playerUIHud.HandleNewHealthValue;
        playerStat.CurrentHealthChange += playerStat.HandleCurrentHealthChange;

        playerStat.DrainingStamina += playerStat.ResetStaminaRegenerationTimer;

        //Weapons
        playerEquipment.LeftHandWeaponIdChange += playerEquipment.HandleCurrentLeftHandWeaponIdChange;
        playerEquipment.RightHandWeaponIdChange += playerEquipment.HandleCurrentRightHandWeaponIdChange;

        //Equipment
        playerEquipment.CurrentWeaponBeingUsedIdChange += playerEquipment.HandleCurrentWeaponUsedIdChange;

        //Combat
        OnIsLockOnChanged += playerCombat.HandleIsLockOnChanged;
    }

    protected override void GetComponents()
    {
        base.GetComponents();

        playerInput = GetComponent<InputReader>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerAnimator = GetComponentInChildren<PlayerAnimator>();
        playerInventory = GetComponent<PlayerInventory>();
        playerStat = GetComponent<PlayerStat>();
        playerSaveData = GetComponent<PlayerSaveData>();
        playerEquipment = GetComponent<PlayerEquipment>();
        playerCombat = GetComponent<PlayerCombat>();
    }

    public override IEnumerator ProcessDeathEvent(bool manualSelectDeathAnimation = false)
    {
        PlayerUI.Instance.playerUIPopup.SendYouDiedPopUp();
        return base.ProcessDeathEvent(manualSelectDeathAnimation);
    }

    public override void ReviveCharacter()
    {
        base.ReviveCharacter();

        playerStat.CurrentHealth = playerStat.maxHealth;
        playerStat.CurrentStamina = playerStat.maxStamina;

        playerAnimator.PlayTargetActionAnimation("Empty", false);

        IsAlive = true;
    }

    private void DebugMenu()
    {
        if (revive)
        {
            revive = false;
            ReviveCharacter();
        }

        if (switchRightWeapon)
        {
            switchRightWeapon = false;
            playerEquipment.SwitchRightWeapon();
        }
    }
}
