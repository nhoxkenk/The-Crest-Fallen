using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryView : MonoBehaviour
{
    public ItemSlot[] slots;

    public static bool isDragging;
    public static ItemSlot originalSlot;

    public event Action<ItemSlot, ItemSlot> OnDrop;

    private void Start()
    {
        foreach (var slot in slots)
        {
            slot.InventoryView = this;
            DraggableItem item = slot.GetComponentInChildren<DraggableItem>();

            item.OnStartDrag += HandleOnStartDrag;
            item.OnPointerDown += HandleOnPointerDown;
        }
    }

    private static void HandleOnStartDrag(ItemSlot slot)
    {
        isDragging = true;
        originalSlot = slot;

        //originalSlot.Label.enabled = false;
    }

    private void HandleOnPointerDown(ItemSlot slot)
    {
        Debug.Log("Clicked");
        if(slot.item is WeaponItem)
        {
            slot.item = slot.item as WeaponItem;
        }
        slot.item.UseItem();
    }

    public void InvokeOnDropEvent(ItemSlot original, ItemSlot newSlot)
    {
        if (newSlot != null)
        {
            HandleOnDrop(original, newSlot);

            OnDrop?.Invoke(original, newSlot);    
        }
        
        if(original != null)
        {
            isDragging = false;
            original.Label.enabled = true;

            original = null;
        }
    }

    //Testing thing out
    private void HandleOnDrop(ItemSlot original, ItemSlot newSlot)
    {
        original.Icon = original.thisSlotDraggableItem.image;
        newSlot.Icon = newSlot.thisSlotDraggableItem.image;

        string originalItemLabel = original.Label.text;
        string originalItemId = original.ItemId;
        Sprite originalIcon = original.BaseSprite;
        ScriptableItem item = original.item;

        original.Label.text = newSlot.Label.text;
        original.ItemId = newSlot.ItemId;
        original.BaseSprite = newSlot.BaseSprite;
        original.Icon.sprite = newSlot.BaseSprite;
        original.item = newSlot.item;

        newSlot.Label.text = originalItemLabel;
        newSlot.ItemId = originalItemId;
        newSlot.BaseSprite = originalIcon;
        newSlot.Icon.sprite = originalIcon;
        newSlot.item = item;
    }
}
