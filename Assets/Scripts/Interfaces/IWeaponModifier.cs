using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponModifier
{
    public float LightAttack01Modifier { get; set; }
    public float HeavyAttack01Modifier { get; set; }
    public float ChargeAttack01Modifier { get; set; }
}
