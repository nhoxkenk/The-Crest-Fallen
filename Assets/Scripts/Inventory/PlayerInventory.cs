using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : CharacterInventory
{
    [Header("Input")]
    [SerializeField] private ScriptableInputReader inputReader;

    private void OnEnable()
    {
        inputReader.DrinkEstusFlask += DrinkEstusFlask;
    }

    private void OnDisable()
    {
        inputReader.DrinkEstusFlask -= DrinkEstusFlask;
    }

    [Header("Current Weapon")]
    public WeaponItem currentRightHandWeapon;
    public WeaponItem currentLeftHandWeapon;

    [Header("Right Hand Quick Slots")]
    public int rightHandWeaponIndex = 0;
    public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[3];

    [Header("Left Hand Quick Slots")]
    public int leftHandWeaponIndex = 0;
    public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[3];

    public void DrinkEstusFlask()
    {
        consumeItem.UsingConsumeItem();
    }
}
