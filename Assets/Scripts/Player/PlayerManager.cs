using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    [Header("Debug")]
    public bool revive;

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

    [HideInInspector] public PlayerInput playerInput;
    [HideInInspector] public PlayerLocomotion playerLocomotion;
    [HideInInspector] public PlayerAnimator playerAnimator;
    [HideInInspector] public PlayerInventory playerInventory;
    [HideInInspector] public PlayerStat playerStat;
    [HideInInspector] public PlayerSaveData playerSaveData;

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

        playerLocomotion.HandleAllMovement();

        playerStat.RegenerateStamina();

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
        playerStat.DrainingStamina += PlayerUI.Instance.playerUIHud.HandleNewStaminaValue;
        playerStat.RegeneratingStamina += PlayerUI.Instance.playerUIHud.HandleNewStaminaValue;

        playerStat.CurrentHealthChange += PlayerUI.Instance.playerUIHud.HandleNewHealthValue;
        playerStat.CurrentHealthChange += playerStat.HandleCurrentHealthChange;

        playerStat.DrainingStamina += playerStat.ResetStaminaRegenerationTimer;
    }

    private void GetComponents()
    {
        playerInput = GetComponent<PlayerInput>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerAnimator = GetComponentInChildren<PlayerAnimator>();
        playerInventory = GetComponent<PlayerInventory>();
        playerStat = GetComponent<PlayerStat>();
        playerSaveData = GetComponent<PlayerSaveData>();
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

        isAlive = true;
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
