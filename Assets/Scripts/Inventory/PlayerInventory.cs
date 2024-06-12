using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : CharacterInventory
{
    [Header("Input")]
    [SerializeField] private ScriptableInputReader inputReader;

    public event UnityAction OnItemChangedCallback = delegate { };

    [SerializeField] private int inventorySpace = 20;
    public List<ScriptableItem> items = new List<ScriptableItem>();

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
    public int rightHandWeaponIndex = -1;
    public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[3];

    [Header("Left Hand Quick Slots")]
    public int leftHandWeaponIndex = -1;
    public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[3];

    protected override void Start()
    {
        base.Start();

        RegisterUserHoldingWeapon();
    }

    private void RegisterUserHoldingWeapon()
    {
        WeaponItem unarmed = Instantiate(AllItemsManager.Instance.unarmedWeapon);
        unarmed.characterHoldThisItem = PlayerManager.Instance;

        currentRightHandWeapon = unarmed;
        currentLeftHandWeapon = unarmed;

        for (int i = 0; i < 3; i++)
        {
            weaponsInRightHandSlots[i] = unarmed;
            weaponsInLeftHandSlots[i] = unarmed;
        }
    }

    public void DrinkEstusFlask()
    {
        consumeItem.UsingConsumeItem();
    }

    public bool AddItem(ScriptableItem item)
    {
        if(items.Count >= inventorySpace)
        {
            return false;
        }
        item.characterHoldThisItem = PlayerManager.Instance;
        items.Add(item);
        OnItemChangedCallback?.Invoke();
        return true;
    }

    public void RemoveItem(ScriptableItem item)
    {
        item.characterHoldThisItem = null;
        items.Remove(item);
        OnItemChangedCallback?.Invoke();
    }
}
