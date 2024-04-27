using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISerializer
{
    public string Serialize<T>(T obj);
    public T Deserialize<T>(string json);
}
