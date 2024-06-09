using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AllItemsManager : Singleton<AllItemsManager>
{
    public WeaponItem unarmedWeapon;

    [Header("Weapons")]
    [SerializeField] private List<WeaponItem> weapons;

    [Header("Items")]
    [SerializeField] private List<ScriptableItem> items = new List<ScriptableItem>();

    protected override void Awake()
    {
        base.Awake();
        InitilizeItemsID();
    }

    private void InitilizeItemsID()
    {
        foreach (var weapon in weapons)
        {
            items.Add(weapon);
        }

        for(int i = 0; i < items.Count; i++)
        {
            items[i].itemID = i;
        }
    }

    public WeaponItem GetWeaponItemById(int id)
    {
        return weapons.FirstOrDefault(weapon => weapon.itemID == id);
    }

    public ConsumeItem GetConsumeItemById(int id)
    {
        return items.FirstOrDefault(weapon => weapon.itemID == id) as ConsumeItem;
    }
}
