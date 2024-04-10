using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon Item")]
public class WeaponItem : Item
{
    public GameObject weaponModelPrefab;
    public bool isUnarmed;

    [Header("One Handed Attack Animations")]
    public string oneHand_Light_Attack_1;
    public string oneHand_Heavy_Attack_1;
}
