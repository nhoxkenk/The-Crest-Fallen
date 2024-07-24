using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerEquipment : CharacterEquipment
{
    public WeaponModelInstantiationSlot rightHandSlot;
    public WeaponModelInstantiationSlot leftHandSlot;

    [Header("Factory")]
    [SerializeField] private WeaponFactory itemFactory;

    [Header("Weapon Manager")]
    public IWeapon rightHandWeaponManager;
    public IWeapon leftHandWeaponManager;

    [Header("Weapon Model")]
    [SerializeField] private GameObject rightHandWeaponModel;
    [SerializeField] private GameObject leftHandWeaponModel;

    [Header("Right Equipment ID")]
    [SerializeField] private int currentRightHandWeaponId;

    [SerializeField] private List<IncreaseCharacterMaxStat> increaseCharacterMaxStats = new List<IncreaseCharacterMaxStat>();

    public int CurrentRightHandWeaponId
    {
        get
        {
            return currentRightHandWeaponId;
        }
        set
        {
            RightHandWeaponIdChange?.Invoke(value);
            currentRightHandWeaponId = value;
        }
    }
    public event Action<int> RightHandWeaponIdChange;

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
            LeftHandWeaponIdChange?.Invoke(value);
            currentLeftHandWeaponId = value;
        }
    }
    public event Action<int> LeftHandWeaponIdChange;

    [Header("Input Reader")]
    [SerializeField] private ScriptableInputReader inputReader;

    protected override void Awake()
    {
        base.Awake();

        InitializeWeaponSlots();
    }

    private void OnEnable()
    {
        inputReader.SwitchRightWeapon += SwitchRightWeapon;
        inputReader.SwitchLeftWeapon += SwitchLeftWeapon;
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

        currentRightHandWeaponId = AllItemsManager.Instance.unarmedWeapon.itemID;
        currentLeftHandWeaponId = AllItemsManager.Instance.unarmedWeapon.itemID;
    }

    public void LoadWeaponOnBothHands()
    {
        LoadLeftWeapon();
        LoadRightWeapon();
    }

    #region Left Hand
    public void SwitchLeftWeapon()
    {
        PlayerManager.Instance.playerAnimator.PlayTargetActionAnimation("Swap_Left_Weapon_01", false, false, true, true);

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
        var currentLeftHandWeaponInInventory = PlayerManager.Instance.playerInventory.currentLeftHandWeapon;
        if (currentLeftHandWeaponInInventory != null)
        {
            leftHandWeaponManager = itemFactory.CreateAndGetItem(PlayerManager.Instance, currentLeftHandWeaponInInventory.itemID);
            leftHandWeaponModel = itemFactory.GetItemModel();
            leftHandSlot.LoadWeaponModel(leftHandWeaponModel);
        }
    }

    public void HandleCurrentLeftHandWeaponIdChange(int newId)
    {
        WeaponItem weaponItem = Instantiate(AllItemsManager.Instance.GetWeaponItemById(newId));
        PlayerManager.Instance.playerInventory.currentLeftHandWeapon = weaponItem;
        LoadLeftWeapon();
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
        var currentRightHandWeaponInInventory = PlayerManager.Instance.playerInventory.currentRightHandWeapon;
        if (currentRightHandWeaponInInventory != null)
        {
            rightHandWeaponManager = itemFactory.CreateAndGetItem(PlayerManager.Instance, currentRightHandWeaponInInventory.itemID);
            rightHandWeaponModel = itemFactory.GetItemModel();
            rightHandSlot.LoadWeaponModel(rightHandWeaponModel);
        }
    }

    public void HandleCurrentRightHandWeaponIdChange(int newId)
    {
        var scriptableWeapon = AllItemsManager.Instance.GetWeaponItemById(newId);
        WeaponItem weaponItem = Instantiate(scriptableWeapon);
        PlayerManager.Instance.playerInventory.currentRightHandWeapon = weaponItem;
        LoadRightWeapon();
        ApplyEffectEnhanceByWeapon((MeleeWeaponItem) weaponItem);
    }

    #endregion

    private void ApplyEffectEnhanceByWeapon(MeleeWeaponItem weaponItem)
    {
        if (weaponItem.IsWeaponHasEffect())
        {
            var enhanceEffect = weaponItem.GetEnhanceStatEffect();
            foreach(var effect in enhanceEffect)
            {
                effect.ProcessEffect(PlayerManager.Instance);
                increaseCharacterMaxStats.Add((IncreaseCharacterMaxStat) effect);
            }
        }
        else
        {
            for(int i = increaseCharacterMaxStats.Count - 1; i >= 0; i--)
            {
                increaseCharacterMaxStats[i].RemoveCharacterStat(PlayerManager.Instance);
                increaseCharacterMaxStats.RemoveAt(i);
            }
        }
    }

    public void HandleCurrentWeaponUsedIdChange(int oldId, int newId)
    {
        WeaponItem weaponItem = Instantiate(AllItemsManager.Instance.GetWeaponItemById(newId));
        PlayerManager.Instance.playerCombat.currentWeaponBeingUsed = weaponItem;
    }

    public void PutBackWeaponAfterConsumeItem()
    {
        Destroy(PlayerManager.Instance.characterEffects.instantiatedFXModel);
        LoadRightWeapon();
    }

    public override void EquipWeapon(WeaponItem weaponItem)
    {
        base.EquipWeapon(weaponItem);

        var inventory = PlayerManager.Instance.playerInventory;

        for (int i = 0; i < 3; i++)
        {
            if (inventory.weaponsInRightHandSlots[i].isUnarmed)
            {
                inventory.weaponsInRightHandSlots[i] = weaponItem;
                return;
            }   
        }
    }
}
