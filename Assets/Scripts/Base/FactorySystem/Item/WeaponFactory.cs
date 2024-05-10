using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Factory/Item Factory/Melee Weapon")]
public class WeaponFactory : ItemFactory
{
    [SerializeField] private Weapon weapon;
    private GameObject instance;

    public override IWeapon CreateAndGetWeapon(CharacterManager characterHoldItem, WeaponItem weaponItem)
    {
        instance = Instantiate(weaponItem.weaponModelPrefab);

        weapon = instance.GetComponent<Weapon>();

        weapon.ApplyWeaponDamage(characterHoldItem, weaponItem);

        return weapon;
    }

    public override GameObject GetWeaponModel()
    {
        return instance;
    }
}
