using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSaveData : MonoBehaviour, IBind<BossData>
{
    [field: SerializeField] public string Id { get; set; } = "0";
    [field: SerializeField] public bool Defeated { get; set ; }

    public BossData data;

    public void Bind(BossData data)
    {
        this.data = data;
        this.data.Id = Id;

        Defeated = data.Defeated;
    }

}
