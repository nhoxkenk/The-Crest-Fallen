using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{
    [HideInInspector] public Transform parentAfterDrag;
    public ItemSlot slot;
    public Image image;
    [SerializeField] private bool isDragging = false;

    public event Action<ItemSlot> OnStartDrag = delegate { };
    public event Action<ItemSlot> OnPointerDown = delegate { };

    private void Start()
    {
        slot = GetComponentInParent<ItemSlot>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        Debug.Log("Drag Started");

        slot = GetComponentInParent<ItemSlot>();

        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;

        OnStartDrag?.Invoke(slot);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        transform.SetParent(parentAfterDrag);        
        image.raycastTarget = true;
    }

    public void SetSlotDraggableItem()
    {
        slot.thisSlotDraggableItem = this;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isDragging)
        {
            OnPointerDown?.Invoke(slot);
        }
    }
}
