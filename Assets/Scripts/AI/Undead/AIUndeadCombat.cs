using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIUndeadCombat : AICharacterCombat
{
    [Header("Character Damage Collider")]
    [SerializeField] private DamageCollider leftHandCollider;
    [SerializeField] private DamageCollider rightHandCollider;

    [Header("Damage")]
    [SerializeField] private int baseDamage = 25;
    [SerializeField] private float attack01DamageModifier = 1.1f;
    [SerializeField] private float attack02DamageModifier = 1.15f;

    public void ApplyAttack01DamageModifier()
    {
        rightHandCollider.physicalDamage = (int)(baseDamage * attack01DamageModifier);
        leftHandCollider.physicalDamage = (int)(baseDamage * attack01DamageModifier);
    }

    public void ApplyAttack02DamageModifier()
    {
        rightHandCollider.physicalDamage = (int)(baseDamage * attack02DamageModifier);
        leftHandCollider.physicalDamage = (int)(baseDamage * attack02DamageModifier);
    }

    public void OpenRightHandCollider()
    {
        rightHandCollider.EnableDamageCollider();
    }

    public void OpenLeftHandCollider()
    {
        leftHandCollider.EnableDamageCollider();
    }

    public void CloseRightHandCollider()
    {
        rightHandCollider?.DisableDamageCollider();
    }

    public void CloseLeftHandCollider()
    {
        leftHandCollider?.DisableDamageCollider();
    }
}
