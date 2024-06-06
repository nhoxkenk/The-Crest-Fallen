using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class AIBossCharacterManager : AICharacterManager
{
    [Header("ID")]
    public int bossID = 0;

    [Header("Percentage HP to Phase Shift")]
    [SerializeField] private float percentHp = 0.6f;

    [Header("Conditions")]
    [SerializeField] private bool hasBeenDefeated = false;
    [SerializeField] private bool hasBeenAwaken = false;

    [Header("Fog walls belong to boss")]
    [SerializeField] private List<FogWallInteractable> fogWalls;

    [HideInInspector] public BossSaveData saveData;
    [HideInInspector] public BossSoundEffect bossSoundEffect;
    [HideInInspector] public BossAnimator bossAnimator; 

    [Header("Special States")]
    [SerializeField] private ScriptableAIState sleepState;
    [SerializeField] private List<ScriptableAIState> combatStanceStatePhaseShift;

    private const string MESSAGE = "GREAT TROLL FELLED";

    [Header("Test Purpose")]
    public bool defeatedBossDebug = false;

    protected override void Awake()
    {
        base.Awake();

        bossAnimator = GetComponent<BossAnimator>();
        bossSoundEffect = GetComponent<BossSoundEffect>();
        sleepState = Instantiate(sleepState);                   //Instantiate a copy of scriptable object

        currentState = sleepState;
    }

    protected override void Start()
    {
        base.Start();

        LoadBossData();

        //Why we got this coroutine here, due to Unity cannot Load at exact time
        StartCoroutine(GetFogWallsFromGameManager()); 

        GameManager.Instance.bosses.Add(this);

        if (hasBeenAwaken)
        {
            foreach (var wall in fogWalls)
            {
                wall.IsActive = true;
            }
        }

        if (hasBeenDefeated)
        {
            IsAlive = false;
            foreach(var wall in fogWalls)
            {
                wall.IsActive = false;
            }
        }
    }

    protected override void BindingCharacterEvents()
    {
        base.BindingCharacterEvents();

        AICharacterCombat.OnPlayingAttackSoundFX += characterSoundEffect.PlayAttackGruntSoundFX;
        AICharacterCombat.OnPlayingAttackSoundFX += bossSoundEffect.PlayWeaponWhooshesSoundFX;

        characterStat.CurrentHealthChange += PhaseShift;
    }

    private void LoadBossData()
    {
        if (saveData.Id.Equals(bossID.ToString()))
        {
            if (saveData.Defeated)
            {
                gameObject.SetActive(false);
                hasBeenDefeated = saveData.Defeated;
            }
        }
    }

    protected override void GetComponents()
    {
        base.GetComponents();
        saveData = GetComponent<BossSaveData>();
    }

    private IEnumerator GetFogWallsFromGameManager()
    {
        while(GameManager.Instance.fogWallSpawners.Count == 0)
        {
            yield return new WaitForEndOfFrame();
        }

        fogWalls = new List<FogWallInteractable>();

        GameManager.Instance.SpawnAllFogWall();

        foreach (var wall in GameManager.Instance.fogWallSpawners)
        {
            if (wall.interactable.ID == bossID && wall.interactable is FogWallInteractable)
            {
                FogWallInteractable fogWallInteractable = wall.interactable as FogWallInteractable;
                fogWalls.Add(fogWallInteractable);
                fogWallInteractable.IsActive = false;
            }
        }
    }

    public void WakeBossUp()
    {
        if(!hasBeenAwaken)
        {
            characterAnimator.PlayTargetActionAnimation(bossAnimator.introAnimation, true);
        }

        hasBeenAwaken = true;
        currentState = idleState;

        foreach (var wall in fogWalls)
        {
            wall.IsActive = true;
        }

        //Spawn a UI HP bar of this boss
        OnBossFightHappen();
    }

    protected override void Update()
    {
        base.Update();

        //debug
        if (defeatedBossDebug)
        {
            defeatedBossDebug = false;
            hasBeenDefeated = true;
            saveData.Defeated = true;
            saveData.data.Defeated = saveData.Defeated;

            //SaveLoadSystem.Instance.SaveGame();
        }
    }

    public override IEnumerator ProcessDeathEvent(bool manualSelectDeathAnimation = false)
    {
        PlayerUI.Instance.playerUIPopup.SendBossDefeatedPopUp(MESSAGE);

        hasBeenAwaken = false;
        hasBeenDefeated = true;

        foreach (var wall in fogWalls)
        {
            wall.IsActive = false;
        }

        return base.ProcessDeathEvent(manualSelectDeathAnimation);
    }

    private void OnBossFightHappen()
    {
        if (hasBeenAwaken)
        {
            GameObject bossHealthBar = Instantiate(PlayerUI.Instance.playerUIHud.bossHealthBarObject, PlayerUI.Instance.playerUIHud.bossHealthBarParent);
            UI_BossHpBar bossHp = bossHealthBar.GetComponentInChildren<UI_BossHpBar>();
            bossHp.EnableBossHPBar(this);
        }
    }

    protected void PhaseShift(float oldHealth, float newHealth)
    {
        Debug.Log(newHealth / (characterStat.maxHealth / 100));
        if(newHealth / (characterStat.maxHealth / 100) < percentHp) 
        {
            if(!IsAlive)
            {
                return;
            }
            characterAnimator.PlayTargetActionAnimation(bossAnimator.phaseShiftAnimation, true);
            combatStanceState = Instantiate(combatStanceStatePhaseShift[0]);
            currentState = combatStanceState;
        }
        
    }
}
