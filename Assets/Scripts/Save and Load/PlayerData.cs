using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData : ISaveable
{
    [field: SerializeField] public string Id { get; set; } = null;
    public Vector3 position;
    public Quaternion rotation;
}
