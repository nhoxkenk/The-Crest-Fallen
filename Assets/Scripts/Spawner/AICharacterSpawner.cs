using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacterSpawner : MonoBehaviour
{
    [Header("Spawner Object Id")]
    [SerializeField] private int ID;

    private void Start()
    {
        GameManager.Instance.AICharacterSpawners.Add(this);
        gameObject.SetActive(false);
    }

    public void AttempToSpawnCharacter()
    {
        PooledObject character = ObjectPool.Instance.GetPooledObject(ID);
        character.transform.position = transform.position;
        character.transform.rotation = transform.rotation;
    }
}
