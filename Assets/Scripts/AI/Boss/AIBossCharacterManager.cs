using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBossCharacterManager : AICharacterManager
{
    public int bossID = 0;
    [SerializeField] private bool hasBeenDefeated = false;
    [SerializeField] private bool hasBeenAwaken = false;
    [SerializeField] private List<FogWallInteractable> fogWalls;
    [HideInInspector] public BossSaveData saveData;

    [Header("Test Purpose")]
    public bool defeatedBossDebug = false;

    protected override void Start()
    {
        base.Start();

        LoadBossData();

        //Why we got this coroutine here, due to Unity cannot Load at exact time
        StartCoroutine(GetFogWallsFromGameManager()); 

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
            }
        }
    }

    public void WakeBossUp()
    {
        if (hasBeenAwaken)
        {
            foreach (var wall in fogWalls)
            {
                wall.IsActive = true;
            }
        }
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

        WakeBossUp();

        if (hasBeenDefeated)
        {
            IsAlive = false;
            foreach (var wall in fogWalls)
            {
                wall.IsActive = false;
            }
        }
    }
}
