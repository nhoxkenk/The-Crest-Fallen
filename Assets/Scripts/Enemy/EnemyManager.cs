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

    [HideInInspector] public CharacterAnimator playerAnimator;
    [HideInInspector] public CharacterStat playerStat;
    [HideInInspector] public CharacterCombat playerCombat;

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
        InitializeStat();
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
    private void InitializeStat()
    {
        //Stamina
        characterStat.maxStamina = characterStat.CalculateStaninaBasedOnEnduranceLevel(characterStat.Endurance);
        characterStat.CurrentStamina = characterStat.maxStamina;
        //Health
        characterStat.maxHealth = characterStat.CalculateHealthBasedOnVitalityLevel(characterStat.Vitality);
        characterStat.CurrentHealth = characterStat.maxHealth;
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
