using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetSword : Interactable
{
    public override void Interact(PlayerManager character)
    {
        base.Interact(character);
        ScriptableItem item = Instantiate(AllItemsManager.Instance.GetWeaponItemById(2));
        character.playerInventory.AddItem(item);
    }
}
