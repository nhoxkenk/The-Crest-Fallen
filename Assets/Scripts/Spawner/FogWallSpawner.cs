using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogWallSpawner : MonoBehaviour
{
    [Header("Spawner Object Id")]
    [SerializeField] private int ID;

    public IInteractable interactable;

    private void Awake()
    {
        GameManager.Instance.fogWallSpawners.Add(this);
        gameObject.SetActive(false);
    }

    public void AttempToSpawnFogWall()
    {
        PooledObject objectPooled = ObjectPool.Instance.GetPooledObject(ID);
        interactable = objectPooled.GetComponent<IInteractable>();
        objectPooled.transform.position = transform.position;
        objectPooled.transform.rotation = transform.rotation;
    }
}
