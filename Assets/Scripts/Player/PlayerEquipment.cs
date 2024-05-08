using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerEquipment : CharacterEquipment
{
    public WeaponModelInstantiationSlot rightHandSlot;
    public WeaponModelInstantiationSlot leftHandSlot;

    [Header("Weapon Manager")]
    [SerializeField] private WeaponManager rightHandWeaponManager;
    [SerializeField] private WeaponManager leftHandWeaponManager;

    [Header("Weapon Model")]
    [SerializeField] private GameObject rightHandWeaponModel;
    [SerializeField] private GameObject leftHandWeaponModel;

    [Header("Right Equipment ID")]
    [SerializeField] private int currentRightHandWeaponId;
    public int CurrentRightHandWeaponId
    {
        get
        {
            return currentRightHandWeaponId;
        }
        set
        {
            RightHandWeaponIdChange?.Invoke(currentRightHandWeaponId, value);
            currentRightHandWeaponId = value;
        }
    }
    public event Action<int, int> RightHandWeaponIdChange;

    [Header("Left Equipment ID")]
    [SerializeField] private int currentLeftHandWeaponId;
    public int CurrentLeftHandWeaponId
    {
        get
        {
            return currentLeftHandWeaponId;
        }
        set
        {
            LeftHandWeaponIdChange?.Invoke(currentLeftHandWeaponId, value);
            currentLeftHandWeaponId = value;
        }
    }
    public event Action<int, int> LeftHandWeaponIdChange;


    protected override void Awake()
    {
        base.Awake();
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

    #region Left Hand
    public void SwitchLeftWeapon()
    {
        PlayerManager.Instance.playerAnimator.PlayTargetActionAnimation("Swap_Left_Weapon_01", false);

        WeaponItem selectedWeapon = null;

        var inventory = PlayerManager.Instance.playerInventory;

        inventory.leftHandWeaponIndex += 1;

        if (inventory.leftHandWeaponIndex < 0 || inventory.leftHandWeaponIndex >= inventory.weaponsInLeftHandSlots.Length)
        {
            inventory.leftHandWeaponIndex = 0;

            //Checking if hold more than one weapon
            int weaponCurrentHold = 0;
            int firstWeaponIndex = 0;
            WeaponItem firstWeapon = null;

            for (int i = 0; i < inventory.weaponsInLeftHandSlots.Length; i++)
            {
                if (inventory.weaponsInLeftHandSlots[i].itemID != AllItemsManager.Instance.unarmedWeapon.itemID)
                {
                    weaponCurrentHold++;

                    if (firstWeapon == null)
                    {
                        firstWeapon = inventory.weaponsInLeftHandSlots[i];
                        firstWeaponIndex = i;
                    }
                }
            }

            if (weaponCurrentHold <= 1)
            {
                inventory.leftHandWeaponIndex = -1;
                selectedWeapon = AllItemsManager.Instance.unarmedWeapon;
                CurrentLeftHandWeaponId = selectedWeapon.itemID;
            }
            else
            {
                inventory.leftHandWeaponIndex = firstWeaponIndex;
                CurrentLeftHandWeaponId = firstWeapon.itemID;
            }
            return;
        }

        if (inventory.weaponsInLeftHandSlots[inventory.leftHandWeaponIndex].itemID != AllItemsManager.Instance.unarmedWeapon.itemID)
        {
            selectedWeapon = inventory.weaponsInLeftHandSlots[inventory.leftHandWeaponIndex];
            CurrentLeftHandWeaponId = selectedWeapon.itemID;
            return;
        }

        if(selectedWeapon == null && inventory.leftHandWeaponIndex < inventory.weaponsInLeftHandSlots.Length)
        {
            SwitchLeftWeapon();
        }
    }

    public void LoadLeftWeapon()
    {
        if(PlayerManager.Instance.playerInventory.currentLeftHandWeapon != null)
        {
            leftHandWeaponModel = Instantiate(PlayerManager.Instance.playerInventory.currentLeftHandWeapon.weaponModelPrefab);
            leftHandSlot.LoadWeaponModel(leftHandWeaponModel);
            leftHandWeaponManager = leftHandWeaponModel.GetComponent<WeaponManager>();
            leftHandWeaponManager.SetWeaponDamage(PlayerManager.Instance, PlayerManager.Instance.playerInventory.currentLeftHandWeapon);
        }
    }

    public void HandleCurrentLeftHandWeaponIdChange(int oldId, int newId)
    {
        WeaponItem weaponItem = Instantiate(AllItemsManager.Instance.GetWeaponItemById(newId));
        PlayerManager.Instance.playerInventory.currentLeftHandWeapon = weaponItem;
        PlayerManager.Instance.playerEquipment.LoadLeftWeapon();
    }

    #endregion


    #region Right Hand
    public void SwitchRightWeapon()
    {
        //Weapon swapping:
        //1. If we have more than one weapon, never swap to unarmed, just rotated between 1 and 2
        //2. If we don't, swap to unarmed, skip other empty slot.

        PlayerManager.Instance.playerAnimator.PlayTargetActionAnimation("Swap_Right_Weapon_01", false, false, true, true);

        WeaponItem selectedWeapon = null;

        var inventory = PlayerManager.Instance.playerInventory;

        inventory.rightHandWeaponIndex += 1;

        if (inventory.rightHandWeaponIndex < 0 || inventory.rightHandWeaponIndex >= inventory.weaponsInRightHandSlots.Length)
        {
            inventory.rightHandWeaponIndex = 0;

            //Checking if hold more than one weapon
            int weaponCurrentHold = 0;
            int firstWeaponIndex = 0;
            WeaponItem firstWeapon = null;

            for (int i = 0; i < inventory.weaponsInRightHandSlots.Length; i++)
            {
                if (inventory.weaponsInRightHandSlots[i].itemID != AllItemsManager.Instance.unarmedWeapon.itemID)
                {
                    weaponCurrentHold++;

                    if (firstWeapon == null)
                    {
                        firstWeapon = inventory.weaponsInRightHandSlots[i];
                        firstWeaponIndex = i;
                    }
                }
            }

            if (weaponCurrentHold <= 1)
            {
                inventory.rightHandWeaponIndex = -1;
                selectedWeapon = AllItemsManager.Instance.unarmedWeapon;
                CurrentRightHandWeaponId = selectedWeapon.itemID;
            }
            else
            {
                inventory.rightHandWeaponIndex = firstWeaponIndex;
                CurrentRightHandWeaponId = firstWeapon.itemID;
            }
            return;
        }

        if (inventory.weaponsInRightHandSlots[inventory.rightHandWeaponIndex].itemID != AllItemsManager.Instance.unarmedWeapon.itemID)
        {
            selectedWeapon = inventory.weaponsInRightHandSlots[inventory.rightHandWeaponIndex];
            CurrentRightHandWeaponId = selectedWeapon.itemID;
            return;
        }

        if (selectedWeapon == null && inventory.rightHandWeaponIndex < inventory.weaponsInRightHandSlots.Length)
        {
            SwitchRightWeapon();
        }
    }

    public void LoadRightWeapon()
    {
        if (PlayerManager.Instance.playerInventory.currentRightHandWeapon != null)
        {
            rightHandWeaponModel = Instantiate(PlayerManager.Instance.playerInventory.currentRightHandWeapon.weaponModelPrefab);
            rightHandSlot.LoadWeaponModel(rightHandWeaponModel);
            rightHandWeaponManager = rightHandWeaponModel.GetComponent<WeaponManager>();
            rightHandWeaponManager.SetWeaponDamage(PlayerManager.Instance, PlayerManager.Instance.playerInventory.currentRightHandWeapon);
        }
    }

    public void HandleCurrentRightHandWeaponIdChange(int oldId, int newId)
    {
        WeaponItem weaponItem = Instantiate(AllItemsManager.Instance.GetWeaponItemById(newId));
        PlayerManager.Instance.playerInventory.currentRightHandWeapon = weaponItem;
        PlayerManager.Instance.playerEquipment.LoadRightWeapon();
    }

    #endregion

    public void HandleCurrentWeaponUsedIdChange(int oldId, int newId)
    {
        WeaponItem weaponItem = Instantiate(AllItemsManager.Instance.GetWeaponItemById(newId));
        PlayerManager.Instance.playerCombat.currentWeaponUsed = weaponItem;
    }
}
