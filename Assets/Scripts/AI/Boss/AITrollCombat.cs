using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AITrollCombat : AICharacterCombat
{
    [Header("Character Damage Collider")]
    [SerializeField] private DamageCollider clubDamageCollider;

    [Header("Damage")]
    [SerializeField] private int baseDamage = 25;
    [SerializeField] private float attack01DamageModifier = 1.1f;
    [SerializeField] private float attack02DamageModifier = 1.15f;
    [SerializeField] private float attack03DamageModifier = 1.35f;

    //Register as animation event
    public override void ApplyAttack01DamageModifier()
    {
        base.ApplyAttack01DamageModifier();
        clubDamageCollider.physicalDamage = (int)(baseDamage * attack01DamageModifier);
    }

    //Register as animation event
    public override void ApplyAttack02DamageModifier()
    {
        base.ApplyAttack02DamageModifier();
        clubDamageCollider.physicalDamage = (int)(baseDamage * attack02DamageModifier);
    }

    //Register as animation event
    public override void ApplyAttack03DamageModifier()
    {
        base.ApplyAttack03DamageModifier();
        clubDamageCollider.physicalDamage = (int)(baseDamage * attack03DamageModifier);
    }

    //Register as animation event
    public void OpenClubWeaponCollider()
    {
        clubDamageCollider.EnableDamageCollider();
    }

    //Register as animation event
    public void CloseClubWeaponCollider()
    {
        clubDamageCollider?.DisableDamageCollider();
    }
}
