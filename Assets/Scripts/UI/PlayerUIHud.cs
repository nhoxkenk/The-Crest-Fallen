using UnityEngine;

public class PlayerUIHud : MonoBehaviour
{
    [SerializeField] private UI_StatBars healthBar;
    [SerializeField] private UI_StatBars staminaBar;

    private void Start()
    {
        PlayerManager.Instance.playerStat.DrainingStamina += HandleNewStaminaValue;
        PlayerManager.Instance.playerStat.RegeneratingStamina += HandleNewStaminaValue;

        PlayerManager.Instance.playerStat.IncreaseHealth += HandleNewHealthValue;
        PlayerManager.Instance.playerStat.DecreaseHealth += HandleNewHealthValue;
    }

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

}
