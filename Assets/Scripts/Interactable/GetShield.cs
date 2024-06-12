using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetShield : Interactable
{
    public override void Interact(PlayerManager character)
    {
        base.Interact(character);
        ScriptableItem item = AllItemsManager.Instance.GetWeaponItemById(3);
        character.playerInventory.AddItem(item);
    }
}
