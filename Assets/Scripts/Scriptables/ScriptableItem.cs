using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableItem : ScriptableObject
{
    [Header("Item Information")]
    public string itemName;
    public Sprite itemIcon;
    [TextArea] public string itemDescription;
    public int itemID;
    [HideInInspector] public CharacterManager characterHoldThisItem;

    public virtual void UseItem()
    {

    }
}
