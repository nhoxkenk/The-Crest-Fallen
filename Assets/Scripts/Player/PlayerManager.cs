using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : CharacterManager
{
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

        playerInput = GetComponent<PlayerInput>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerAnimator = GetComponentInChildren<PlayerAnimator>();
        playerInventory = GetComponent<PlayerInventory>();
        playerStat = GetComponent<PlayerStat>();
        playerSaveData = GetComponent<PlayerSaveData>();
    }

    protected override void Start()
    {
        base.Start();

        InitializePlayerStat();
    }

    protected override void Update()
    {
        base.Update();

        playerLocomotion.HandleAllMovement();

        playerStat.RegenerateStamina();
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
}
