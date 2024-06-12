using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Factory/Item Factory/Melee Weapon")]
public class SwordFactory : WeaponFactory
{
    [SerializeField] private Weapon weapon;
    private GameObject instance;

    public override IWeapon CreateAndGetItem(CharacterManager characterHoldItem, int itemID)
    {
        var weaponItem = Instantiate(AllItemsManager.Instance.GetWeaponItemById(itemID));

        instance = Instantiate(weaponItem.weaponModelPrefab);

        weapon = instance.GetComponent<Weapon>();

        weapon.ApplyWeaponDamage(characterHoldItem, weaponItem);

        return weapon;
    }

    public override GameObject GetItemModel()
    {
        return instance;
    }
}
