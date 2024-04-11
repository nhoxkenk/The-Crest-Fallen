using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIHud : MonoBehaviour
{
    [SerializeField] private UI_StatBars staminaBar;
    [SerializeField] private PlayerManager playerManager;

    private void Start()
    {
        playerManager.playerStat.OnStaminaDrain += SetNewStaminaValue;
        playerManager.playerStat.OnStaminaDrain += ResetStaminaRegenerationTimer;
        playerManager.playerStat.OnStaminaRegenerate += SetNewStaminaValue;
    }

    private void OnDisable()
    {
        playerManager.playerStat.OnStaminaDrain -= SetNewStaminaValue;
        playerManager.playerStat.OnStaminaDrain -= ResetStaminaRegenerationTimer;
        playerManager.playerStat.OnStaminaRegenerate -= SetNewStaminaValue;
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
