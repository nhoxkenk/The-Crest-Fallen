using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Debug")]
    public bool spawnCharacter;

    [Header("AI")]
    public List<AICharacterSpawner> spawners;

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
        foreach(var character in spawners)
        {
            character.AttempToSpawnCharacter();
        }
    }
}
