using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObject : MonoBehaviour
{
    [field: SerializeField] public int ObjectID { get; set; }
    private ObjectPool pool;
    public ObjectPool Pool { get { return pool; } set => pool = value; }

    public void Release()
    {
        pool.ReturnToPool(this, ObjectID);
    }
}
