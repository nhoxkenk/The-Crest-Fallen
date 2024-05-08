using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEquipment : MonoBehaviour
{
    [Header("Current Weapon ID")]
    [SerializeField] private int currentWeaponBeingUsedId;
    public int CurrentWeaponBeingUsedId
    {
        get
        {
            return currentWeaponBeingUsedId;
        }
        set
        {
            CurrentWeaponBeingUsedIdChange?.Invoke(currentWeaponBeingUsedId, value);
            currentWeaponBeingUsedId = value;
        }
    }
    public event Action<int, int> CurrentWeaponBeingUsedIdChange;

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {

    }
}
