using UnityEngine;
using UnityEngine.UI;

public class PlayerUIHud : MonoBehaviour
{
    [Header("Stat Bars")]
    [SerializeField] private UI_StatBars healthBar;
    [SerializeField] private UI_StatBars staminaBar;

    [Header("Quick Slots")]
    [SerializeField] private Image rightWeaponQuickSlotImage;
    [SerializeField] private Image leftWeaponQuickSlotImage;

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
}
