using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerEquipment : CharacterEquipment
{
    private PlayerManager playerManager;
    public WeaponModelInstantiationSlot rightHandSlot;
    public WeaponModelInstantiationSlot leftHandSlot;

    [SerializeField] private GameObject rightHandWeaponModel;
    [SerializeField] private GameObject leftHandWeaponModel;

    protected override void Awake()
    {
        base.Awake();
        playerManager = GetComponent<PlayerManager>();
        InitializeWeaponSlots();
    }

    protected override void Start()
    {
        base.Start();
        LoadWeaponOnBothHands();
    }

    private void InitializeWeaponSlots()
    {
        WeaponModelInstantiationSlot[] weaponSlots = GetComponentsInChildren<WeaponModelInstantiationSlot>();
        foreach(WeaponModelInstantiationSlot slot in weaponSlots)
        {
            if(slot.weaponSlot == WeaponModelSlot.RightHand)
            {
                rightHandSlot = slot;
            }
            if (slot.weaponSlot == WeaponModelSlot.LeftHand)
            {
                leftHandSlot = slot;
            }
        }
    }

    public void LoadWeaponOnBothHands()
    {
        LoadLeftWeapon();
        LoadRightWeapon();
    }

    public void LoadLeftWeapon()
    {
        if(playerManager.playerInventory.currentLeftHandWeapon != null)
        {
            rightHandWeaponModel = Instantiate(playerManager.playerInventory.currentLeftHandWeapon.weaponModelPrefab);
            leftHandSlot.LoadWeaponModel(rightHandWeaponModel);
        }
    }

    public void LoadRightWeapon()
    {
        if (playerManager.playerInventory.currentRightHandWeapon != null)
        {
            leftHandWeaponModel = Instantiate(playerManager.playerInventory.currentRightHandWeapon.weaponModelPrefab);
            rightHandSlot.LoadWeaponModel(leftHandWeaponModel);
        }
    }
}
