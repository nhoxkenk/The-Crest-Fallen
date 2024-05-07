using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : CharacterInventory
{

    public WeaponItem currentRightHandWeapon;
    public WeaponItem currentLeftHandWeapon;

    [Header("Right Hand Quick Slots")]
    public int rightHandWeaponIndex = 0;
    public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[3];

    [Header("Left Hand Quick Slots")]
    public int leftHandWeaponIndex = 0;
    public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[3];
}
