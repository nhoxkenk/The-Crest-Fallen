using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BossData : ISaveable
{
    [field: SerializeField] public string Id { get; set; }
    [field: SerializeField] public bool Defeated { get; set; }
}
