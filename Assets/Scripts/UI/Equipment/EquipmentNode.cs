using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentNode : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private ItemSlot slot;
    [SerializeField] private EquipmentType equipmentType;
    private Color color;

    private void Start()
    {
        color = iconImage.color;
    }

    public void DisplayEquipment(ItemSlot slot)
    {
        this.slot = slot;
        iconImage.sprite = slot.BaseSprite;
        iconImage.color = Color.white;
    }

    public void RemoveEquipmentDisplay()
    {
        
    }
}
