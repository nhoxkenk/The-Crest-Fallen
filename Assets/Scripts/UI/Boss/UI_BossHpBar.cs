using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_BossHpBar : UI_StatBars
{
    [SerializeField] private AIBossCharacterManager bossCharacter;

    public void EnableBossHPBar(AIBossCharacterManager boss)
    {
        bossCharacter = boss;
        bossCharacter.characterStat.CurrentHealthChange += OnBossHPChanged;
        SetMaxStat((int) bossCharacter.characterStat.maxHealth);
        SetMaxStat((int)bossCharacter.characterStat.CurrentHealth);
        GetComponentInChildren<TextMeshProUGUI>().text = bossCharacter.name;
    }

    private void OnDestroy()
    {
        bossCharacter.characterStat.CurrentHealthChange -= OnBossHPChanged;
    }

    public void OnBossHPChanged(float oldValue, float newValue)
    {
        SetStat((int) newValue);

        if(newValue <= 0)
        {
            RemoveHpBar(2f);
        }
    }

    public void RemoveHpBar(float time)
    {
        Destroy(gameObject, time);
    }
}
