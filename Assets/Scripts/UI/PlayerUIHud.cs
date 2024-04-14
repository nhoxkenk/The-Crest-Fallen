using UnityEngine;

public class PlayerUIHud : MonoBehaviour
{
    [SerializeField] private UI_StatBars staminaBar;

    private void Start()
    {
        PlayerManager.Instance.playerStat.DrainingStamina += HandleNewStaminaValue;
        PlayerManager.Instance.playerStat.DrainingStamina += ResetStaminaRegenerationTimer;
        PlayerManager.Instance.playerStat.RegeneratingStamina += HandleNewStaminaValue;
    }

    public void HandleNewStaminaValue(float oldValue, float newValue)
    {
        staminaBar.SetStat(Mathf.RoundToInt(newValue));
    }

    public void HandleMaxStaminaValue(float value)
    {
        staminaBar.SetMaxStat(Mathf.RoundToInt(value));
    }

    public void ResetStaminaRegenerationTimer(float oldValue, float newValue)
    {
        if (newValue < oldValue)
        {
            PlayerManager.Instance.playerStat.staminaRegenerationTimer = 0;
        }
    }
}
