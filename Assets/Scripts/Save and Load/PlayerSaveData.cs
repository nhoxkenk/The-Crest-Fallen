using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSaveData : MonoBehaviour, IBind<PlayerData>
{
    [field: SerializeField] public string Id { get; set; } = "Test Id";
    [field: SerializeField] public string CharacterName { get; set; } = "Character Name";

    public PlayerData data;

    public void Bind(PlayerData data)
    {
        this.data = data;
        this.data.Id = Id;
        this.data.CharacterName = CharacterName;

        transform.position = data.position;
        transform.rotation = data.rotation;

        if (data.vitality != 0)
        {
            PlayerManager.Instance.playerStat.Vitality = data.vitality;
        }
        this.data.vitality = PlayerManager.Instance.playerStat.Vitality;

        if (data.endurance != 0)
        {
            PlayerManager.Instance.playerStat.Endurance = data.endurance;
        }
        this.data.endurance = PlayerManager.Instance.playerStat.Endurance;

        if (data.currentHealth != 0)
        {
            this.data.currentHealth = data.currentHealth;
        }
        else
        {
            PlayerManager.Instance.playerStat.maxHealth = PlayerManager.Instance.playerStat.CalculateHealthBasedOnVitalityLevel(PlayerManager.Instance.playerStat.Vitality);
            PlayerManager.Instance.playerStat.CurrentHealth = PlayerManager.Instance.playerStat.maxHealth;
            this.data.currentHealth = PlayerManager.Instance.playerStat.CurrentHealth;
        }

        if (data.currentStamina != 0)
        {
            this.data.currentStamina = data.currentStamina;
        }
        else
        {
            PlayerManager.Instance.playerStat.maxStamina = PlayerManager.Instance.playerStat.CalculateStaninaBasedOnEnduranceLevel(PlayerManager.Instance.playerStat.Endurance);
            PlayerManager.Instance.playerStat.CurrentStamina = PlayerManager.Instance.playerStat.maxStamina;
            this.data.currentStamina = PlayerManager.Instance.playerStat.CurrentStamina;
        }
    }

    private void Update()
    {
        data.position = transform.position;
        data.rotation = transform.rotation;
        
        data.vitality = PlayerManager.Instance.playerStat.Vitality;
        data.endurance = PlayerManager.Instance.playerStat.Endurance;

        data.currentHealth = PlayerManager.Instance.playerStat.CurrentHealth;
        data.currentStamina = PlayerManager.Instance.playerStat.CurrentStamina;
    }
}
