using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
    [Header("Attribute")]
    [SerializeField] private int poolSize;
    [SerializeField] private List<PooledObject> objectToPool;

    private Dictionary<int, Stack<PooledObject>> poolMap;

    protected override void Awake()
    {
        SetupPool();
        base.Awake();    
    }

    private void SetupPool()
    {
        poolMap = new Dictionary<int, Stack<PooledObject>>();

        PooledObject instance = null;

        for(int j  = 0; j < objectToPool.Count; j++)
        {
            Stack<PooledObject> stack = new Stack<PooledObject>();

            for (int i = 0; i < poolSize; i++)
            {
                instance = Instantiate(objectToPool[j]);
                instance.Pool = this;
                instance.gameObject.SetActive(false);
                instance.gameObject.transform.SetParent(gameObject.transform, false);
                stack.Push(instance);
            }

            poolMap.Add(objectToPool[j].ObjectID, stack);
        }
        
    }

    public PooledObject GetPooledObject(int id)
    {
        if(!poolMap.ContainsKey(id))
        {
            Debug.LogError($"Pool with ID {id} does not exist.");
            return null;
        }

        Stack<PooledObject> stack = poolMap[id];

        if (stack.Count == 0)
        {
            PooledObject newInstance = Instantiate(objectToPool[id]);
            newInstance.Pool = this;
            return newInstance;
        }
        // otherwise, just grab the next one from the list

        PooledObject nextInstance = stack.Pop();
        nextInstance.gameObject.SetActive(true);
        return nextInstance;
    }

    public void ReturnToPool(PooledObject pooledObject, int id)
    {
        if (!poolMap.ContainsKey(id))
        {
            Debug.LogError($"Pool with ID {id} does not exist.");
            Destroy(pooledObject.gameObject);
            return;
        }

        pooledObject.gameObject.SetActive(false);
        poolMap[id].Push(pooledObject);
    }
}
