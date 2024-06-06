using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Debug")]
    public bool spawnCharacter;

    [Header("AI")]
    public List<AICharacterSpawner> AICharacterSpawners;

    [Header("Fog Wall")]
    public List<FogWallSpawner> fogWallSpawners;

    [Header("Bosses")]
    public List<AIBossCharacterManager> bosses;

    protected override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        if (spawnCharacter)
        {
            spawnCharacter = false;
            SpawnAllCharacter();
        }
    }

    public void SpawnAllCharacter()
    {
        foreach(var character in AICharacterSpawners)
        {
            character.AttempToSpawnCharacter();
        }
    }

    public void SpawnAllFogWall()
    {
        foreach (var fogWall in fogWallSpawners)
        {
            fogWall.AttempToSpawnFogWall();
        }
    }

    public AIBossCharacterManager GetBossCharacterByID(int id)
    {
        return bosses.FirstOrDefault(boss => boss.bossID == id);
    }
}
