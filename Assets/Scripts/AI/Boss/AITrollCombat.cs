using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITrollCombat : AICharacterCombat
{
    [Header("Character Damage Collider")]
    [SerializeField] private DamageCollider clubDamageCollider;

    [Header("Damage")]
    [SerializeField] private int baseDamage = 25;
    [SerializeField] private float attack01DamageModifier = 1.1f;
    [SerializeField] private float attack02DamageModifier = 1.15f;
    [SerializeField] private float attack03DamageModifier = 1.35f;

    public void ApplyAttack01DamageModifier()
    {
        clubDamageCollider.physicalDamage = (int)(baseDamage * attack01DamageModifier);
    }

    public void ApplyAttack02DamageModifier()
    {
        clubDamageCollider.physicalDamage = (int)(baseDamage * attack02DamageModifier);
    }

    public void ApplyAttack03DamageModifier()
    {
        clubDamageCollider.physicalDamage = (int)(baseDamage * attack03DamageModifier);
    }

    public void OpenClubWeaponCollider()
    {
        clubDamageCollider.EnableDamageCollider();
    }

    public void CloseClubWeaponCollider()
    {
        clubDamageCollider?.DisableDamageCollider();
    }
}
