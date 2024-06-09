using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumeItem : ScriptableItem
{
    public bool isStackable;

    [Header("Character Holding")]
    public CharacterManager characterHolding;

    [Header("Item Quantity")]
    public int quantity = 1;
    [SerializeField] private int maxQuantity = 1;

    [Header("Item Model")]
    public GameObject itemModel;

    public virtual void UsingConsumeItem()
    {
        quantity--;
    }
}
