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
}
