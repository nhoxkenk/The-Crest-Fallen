using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
    [SerializeField] private int poolSize;
    [SerializeField] private PooledObject objectToPool;

    private Stack<PooledObject> stack;

    protected override void Awake()
    {
        base.Awake();

        SetupPool();
    }

    private void SetupPool()
    {
        stack = new Stack<PooledObject>();
        PooledObject instance = null;

        for(int i = 0; i < poolSize; i++)
        {
            instance = Instantiate(objectToPool);
            instance.Pool = this;
            //instance.gameObject.SetActive(false);
            instance.gameObject.transform.SetParent(gameObject.transform, false);
            stack.Push(instance);
        }
    }

    public PooledObject GetPooledObject()
    {
        if (stack.Count == 0)
        {
            PooledObject newInstance = Instantiate(objectToPool);
            newInstance.Pool = this;
            return newInstance;
        }
        // otherwise, just grab the next one from the list

        PooledObject nextInstance = stack.Pop();
        nextInstance.gameObject.SetActive(true);
        return nextInstance;
    }

    public void ReturnToPool(PooledObject pooledObject)
    {
        stack.Push(pooledObject);
        pooledObject.gameObject.SetActive(false);
    }
}
