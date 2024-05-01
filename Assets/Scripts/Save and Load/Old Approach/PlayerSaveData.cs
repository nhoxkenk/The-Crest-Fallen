using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSaveData : MonoBehaviour, IBind<PlayerData>
{
    [field: SerializeField] public string Id { get; set; } = "Test Id";
    [field: SerializeField] public string CharacterName { get; set; } = "Character Name";

    [SerializeField] private PlayerData data;

    public void Bind(PlayerData data)
    {
        this.data = data;
        this.data.Id = Id;
        this.data.CharacterName = CharacterName;
        transform.position = data.position;
        transform.rotation = data.rotation;

        this.data.vitality = data.vitality;
        this.data.endurance = data.endurance;

        this.data.currentHealth = data.currentHealth;
        this.data.currentStamina = data.currentStamina;
    }

    private void Update()
    {
        data.position = transform.position;
        data.rotation = transform.rotation;
        
        data.vitality = PlayerManager.Instance.playerStat.Vitality;
        data.endurance = PlayerManager.Instance.playerStat.Endurance;

        data.currentHealth = PlayerManager.Instance.playerStat.currentHealth;
        data.currentStamina = PlayerManager.Instance.playerStat.currentStamina;
    }
}
