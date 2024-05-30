using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBossCharacterManager : AICharacterManager
{
    public int bossID = 0;
    [SerializeField] private bool hasBeenDefeated = false;

    [HideInInspector] public BossSaveData saveData;

    [Header("Test Purpose")]
    public bool defeatedBossDebug = false;

    protected override void Start()
    {
        base.Start();

        //if (!SaveLoadSystem.Instance.gameData.bossData.bossesAwaken.ContainsKey(bossID))
        //{
        //    Debug.Log("True");
        //    SaveLoadSystem.Instance.gameData.bossData.bossesAwaken.Add(bossID, false);
        //    SaveLoadSystem.Instance.gameData.bossData.bossesDefeated.Add(bossID, false);
        //}
        //else
        //{
        //    Debug.Log("False");
        //    hasBeenDefeated = SaveLoadSystem.Instance.gameData.bossData.bossesDefeated[bossID];
        //    if (hasBeenDefeated)
        //    {
        //        gameObject.SetActive(false);
        //    }
        //}

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

    protected override void Update()
    {
        base.Update();

        if (defeatedBossDebug)
        {
            defeatedBossDebug = false;
            hasBeenDefeated = true;
            saveData.Defeated = true;
            saveData.data.Defeated = saveData.Defeated;

            SaveLoadSystem.Instance.SaveGame();
        }
    }
}
