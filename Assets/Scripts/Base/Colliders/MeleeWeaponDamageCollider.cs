using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponDamageCollider : DamageCollider, IWeaponModifier
{
    [Header("Weapon Attack Modifiers")]
    private float lightAttack01Modifier;
    public float LightAttack01Modifier { get => lightAttack01Modifier; set => lightAttack01Modifier = value; }

    protected override void Awake()
    {
        base.Awake();

        DisableDamageCollider();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        IEffectable characterEffectable = other.GetComponentInParent<IEffectable>();

        if (characterEffectable != null)
        {
            if (other.GetComponentInParent<CharacterManager>() == characterCausingDamage)
            {
                return;
            }
            contactPoint = other.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            DamageTarget(characterEffectable);
        }
    }

    protected override void DamageTarget(IEffectable characterEffectable)
    {
        if (characterDamaged.Contains(characterEffectable))
        {
            return;
        }

        characterDamaged.Add(characterEffectable);

        TakeHealthDamageEffect damageEffect = Instantiate(CharacterEffectsManager.Instance.takeDamageEffect);
        damageEffect.physicalDamage = physicalDamage;
        damageEffect.magicDamage = magicDamage;
        damageEffect.fireDamage = fireDamage;
        damageEffect.lightningDamage = lightningDamage;
        damageEffect.holyDamage = holyDamage;
        damageEffect.contactPoint = contactPoint;

        switch (characterCausingDamage.characterCombat.currentAttackType)
        {
            case AttackType.lightAttack01:
                ApplyAttackDamageModifiers(LightAttack01Modifier, damageEffect);
                break;
            default:
                break;
        }

        characterEffectable.ProcessInstantEffects(damageEffect); 
    }

    private void ApplyAttackDamageModifiers(float modifier, TakeHealthDamageEffect takeHealthDamageEffect)
    {
        takeHealthDamageEffect.physicalDamage *= modifier;
        takeHealthDamageEffect.magicDamage *= modifier;
        takeHealthDamageEffect.fireDamage *= modifier;
        takeHealthDamageEffect.holyDamage *= modifier;
        takeHealthDamageEffect.poiseDamage *= modifier;
    }
}
