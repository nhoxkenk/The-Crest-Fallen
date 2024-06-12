using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

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
    [field:SerializeField] public bool SwitchToWalking { get; set; }

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

    [HideInInspector] public PlayerLocomotion playerLocomotion;
    [HideInInspector] public PlayerAnimator playerAnimator;
    [HideInInspector] public PlayerInventory playerInventory;
    [HideInInspector] public PlayerStat playerStat;
    [HideInInspector] public PlayerSaveData playerSaveData;
    [HideInInspector] public PlayerEquipment playerEquipment;
    [HideInInspector] public PlayerCombat playerCombat;
    [HideInInspector] public PlayerInteraction playerInteraction;

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
        SetItemUIImage();
    }

    protected override void Update()
    {
        base.Update();

        if (IsAlive)
        {
            playerLocomotion.HandleAllMovement();

            playerAnimator.UpdateAnimator();

            playerStat.RegenerateStamina();

            playerCombat.HandleAllHoldingInputAction();
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
        playerEquipment.LeftHandWeaponIdChange += PlayerUI.Instance.playerUIHud.SetLeftWeaponQuickSlotImage;

        playerEquipment.RightHandWeaponIdChange += playerEquipment.HandleCurrentRightHandWeaponIdChange;
        playerEquipment.RightHandWeaponIdChange += PlayerUI.Instance.playerUIHud.SetRightWeaponQuickSlotImage;

        //Equipment
        playerEquipment.CurrentWeaponBeingUsedIdChange += playerEquipment.HandleCurrentWeaponUsedIdChange;

        //Combat
        OnIsLockOnChanged += playerCombat.HandleIsLockOnChanged;
        playerCombat.OnIsChargingAttack += playerAnimator.HandleIsChargingAttack;
    }

    protected override void GetComponents()
    {
        base.GetComponents();

        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerAnimator = GetComponentInChildren<PlayerAnimator>();
        playerInventory = GetComponent<PlayerInventory>();
        playerStat = GetComponent<PlayerStat>();
        playerSaveData = GetComponent<PlayerSaveData>();
        playerEquipment = GetComponent<PlayerEquipment>();
        playerCombat = GetComponent<PlayerCombat>();
        playerInteraction = GetComponent<PlayerInteraction>();
    }

    private void SetItemUIImage()
    {
        //Estus Flask id equal 0
        PlayerUI.Instance.playerUIHud.SetConsumeItemImage(0);
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
    }
}
