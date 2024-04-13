using UnityEngine;

public class PlayerUIHud : MonoBehaviour
{
    [SerializeField] private UI_StatBars staminaBar;
    [SerializeField] private PlayerManager playerManager;

    private void Start()
    {
        playerManager.playerStat.DrainingStamina += SetNewStaminaValue;
        playerManager.playerStat.DrainingStamina += ResetStaminaRegenerationTimer;
        playerManager.playerStat.RegeneratingStamina += SetNewStaminaValue;
    }

    private void OnDisable()
    {
        playerManager.playerStat.DrainingStamina -= SetNewStaminaValue;
        playerManager.playerStat.DrainingStamina -= ResetStaminaRegenerationTimer;
        playerManager.playerStat.RegeneratingStamina -= SetNewStaminaValue;
    }

    public void SetNewStaminaValue(float oldValue, float newValue)
    {
        staminaBar.SetStat(Mathf.RoundToInt(newValue));
    }

    public void SetMaxStaminaValue(float value)
    {
        staminaBar.SetMaxStat(Mathf.RoundToInt(value));
    }

    public void ResetStaminaRegenerationTimer(float oldValue, float newValue)
    {
        if (newValue < oldValue)
        {
            playerManager.playerStat.staminaRegenerationTimer = 0;
        }
    }
}
