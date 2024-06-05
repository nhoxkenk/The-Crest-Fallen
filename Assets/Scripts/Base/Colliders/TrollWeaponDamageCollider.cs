using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrollWeaponDamageCollider : DamageCollider
{
    protected override void Awake()
    {
        base.Awake();

        characterCausingDamage = GetComponentInParent<AICharacterManager>();
    }

    protected override void DamageTarget(CharacterManager characterEffectable)
    {
        if (characterDamaged.Contains(characterEffectable) || !characterCausingDamage.CanDealDamageTo(characterEffectable))
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
        damageEffect.angleHitFrom = Vector3.SignedAngle(characterCausingDamage.transform.forward, characterEffectable.transform.forward, Vector3.up);

        characterEffectable.characterEffects.ProcessInstantEffects(damageEffect);
    }
}
