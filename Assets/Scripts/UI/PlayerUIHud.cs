using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class PlayerUIHud : MonoBehaviour
{
    [Header("Stat Bars")]
    [SerializeField] private UI_StatBars healthBar;
    [SerializeField] private UI_StatBars staminaBar;

    [Header("Quick Slots")]
    [SerializeField] private Image rightWeaponQuickSlotImage;
    [SerializeField] private Image leftWeaponQuickSlotImage;

    [Header("Consume Slots")]
    [SerializeField] private Image consumeItemImage;
    [SerializeField] private TextMeshProUGUI consumeItemText;

    [Header("Boss Health Bar")]
    public Transform bossHealthBarParent;
    public GameObject bossHealthBarObject;

    public void RefreshHUD()
    {
        healthBar.gameObject.SetActive(false);
        healthBar.gameObject.SetActive(true);

        staminaBar.gameObject.SetActive(false);
        staminaBar.gameObject.SetActive(true);
    }

    public void HandleNewHealthValue(float oldValue, float newValue)
    {
        healthBar.SetStat(Mathf.RoundToInt(newValue));
    }

    public void HandleMaxHealthValue(float value)
    {
        healthBar.SetMaxStat(Mathf.RoundToInt(value));
    }

    public void HandleNewStaminaValue(float oldValue, float newValue)
    {
        staminaBar.SetStat(Mathf.RoundToInt(newValue));
    }

    public void HandleMaxStaminaValue(float value)
    {
        staminaBar.SetMaxStat(Mathf.RoundToInt(value));
    }

    public void SetRightWeaponQuickSlotImage(int weaponId)
    {
        WeaponItem weapon = AllItemsManager.Instance.GetWeaponItemById(weaponId);
        if (weapon == null || weapon.itemIcon == null)
        {
            rightWeaponQuickSlotImage.enabled = false;
            rightWeaponQuickSlotImage.sprite = null;
            return;
        }

        rightWeaponQuickSlotImage.sprite = weapon.itemIcon;
        rightWeaponQuickSlotImage.enabled = true;
    }

    public void SetLeftWeaponQuickSlotImage(int weaponId)
    {
        WeaponItem weapon = AllItemsManager.Instance.GetWeaponItemById(weaponId);
        if (weapon == null || weapon.itemIcon == null)
        {
            leftWeaponQuickSlotImage.enabled = false;
            leftWeaponQuickSlotImage.sprite = null;
            return;
        }

        leftWeaponQuickSlotImage.sprite = weapon.itemIcon;
        leftWeaponQuickSlotImage.enabled = true;
    }

    public void SetConsumeItemImage(int itemId)
    {
        ConsumeItem item = AllItemsManager.Instance.GetConsumeItemById(itemId);
        if (item == null || item.itemIcon == null)
        {
            consumeItemImage.enabled = false;
            consumeItemImage.sprite = null;
            consumeItemText.text = "0";
            return;
        }

        consumeItemImage.sprite = item.itemIcon;
        consumeItemText.text = item.quantity.ToString();
        consumeItemImage.enabled = true;
    }

    public void HandleConsumeItemChanged(ConsumeItem item)
    {
        consumeItemImage.color = item.quantity == 0
            ? new Color32(255, 255, 225, 100)
            : new Color32(255, 255, 225, 255);

        if (item.quantity >= 0)
        {
            consumeItemImage.sprite = item.itemIcon;
            consumeItemText.text = item.quantity.ToString();
        }
    }
}
