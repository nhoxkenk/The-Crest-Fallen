using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacterSpawner : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.spawners.Add(this);
        gameObject.SetActive(false);
    }

    public void AttempToSpawnCharacter()
    {
        PooledObject character = ObjectPool.Instance.GetPooledObject();
        character.transform.position = transform.position;
        //character.transform.rotation = transform.rotation;
    }
}
