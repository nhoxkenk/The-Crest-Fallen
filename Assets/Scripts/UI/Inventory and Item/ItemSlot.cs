using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public Image Icon;
    public TextMeshProUGUI Label;
    public string ItemId;
    public Sprite BaseSprite;

    [HideInInspector] public InventoryView InventoryView;

    public DraggableItem thisSlotDraggableItem;
    public ScriptableItem item;

    private void Start()
    {
        thisSlotDraggableItem = GetComponentInChildren<DraggableItem>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        DraggableItem itemDraggableImage = dropped.GetComponent<DraggableItem>();
        if (itemDraggableImage != null)
        {
            thisSlotDraggableItem = GetComponentInChildren<DraggableItem>();

            SwapDraggableItemGameObject(itemDraggableImage, thisSlotDraggableItem);

            InventoryView.InvokeOnDropEvent(InventoryView.originalSlot, this);
        }
    }

    private void SwapDraggableItemGameObject(DraggableItem newItem, DraggableItem currentItem)
    {
        if (currentItem != null)
        {
            currentItem.parentAfterDrag = newItem.parentAfterDrag;
            currentItem.transform.SetParent(newItem.parentAfterDrag);
            currentItem.slot = newItem.slot;
            currentItem.SetSlotDraggableItem();
            currentItem.image.raycastTarget = true;
        }

        newItem.parentAfterDrag = transform;
        newItem.slot = this;
        newItem.SetSlotDraggableItem();
    }

    public void AddItemToSlot(ScriptableItem item)
    {
        this.item = item;

        ItemId = item.itemID.ToString();
        Icon.sprite = item.itemIcon;
        Icon.enabled = true;
        BaseSprite = item.itemIcon;
    }

    public void ClearSlot()
    {
        this.item = null;

        ItemId = "";
        Icon.sprite = null;
        Icon.enabled = false;
        BaseSprite = null;
    }
}
