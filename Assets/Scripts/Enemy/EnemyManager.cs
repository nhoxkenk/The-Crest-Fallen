using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : CharacterManager
{
    [Header("Debug")]
    public bool revive;
    public bool switchRightWeapon;

    private static EnemyManager instance;
    public static EnemyManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EnemyManager>();
            }
            return instance;
        }
    }

    [HideInInspector] public PlayerAnimator playerAnimator;
    [HideInInspector] public PlayerStat playerStat;
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
        //BindingPlayerEvents();
        InitializePlayerStat();
    }

    protected override void Update()
    {
        base.Update();

        //playerStat.RegenerateStamina();

        DebugMenu();
    }

    private void LateUpdate()
    {
        //PlayerCamera.Instance.HandleAllCameraActions();
    }

    /// <summary>
    /// Initialize Player Stat
    /// </summary>
    private void InitializePlayerStat()
    {
        //Stamina
        playerStat.maxStamina = playerStat.CalculateStaninaBasedOnEnduranceLevel(playerStat.Endurance);
        playerStat.CurrentStamina = playerStat.maxStamina;
        PlayerUI.Instance.playerUIHud.HandleMaxStaminaValue(playerStat.maxStamina);
        playerStat.IncreaseVitalityStat += playerStat.OnIncreaseVitalityStat;

        //Health
        playerStat.maxHealth = playerStat.CalculateHealthBasedOnVitalityLevel(playerStat.Vitality);
        playerStat.CurrentHealth = playerStat.maxHealth;
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
    }

    protected override void GetComponents()
    {
        base.GetComponents();

        playerAnimator = GetComponentInChildren<PlayerAnimator>();
        playerStat = GetComponent<PlayerStat>();
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
        }
    }
}
