using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    [Header("Damage")]
    public float physicalDamage;
    public float magicDamage;
    public float fireDamage;
    public float lightningDamage;
    public float holyDamage;

    [Header("Contact Point")]
    [SerializeField] private Vector3 contactPoint;

    [Header("Character Damaged")]
    protected List<CharacterManager> characterDamaged = new List<CharacterManager>();

    private void OnTriggerEnter(Collider other)
    {
        CharacterManager characterManager = other.GetComponent<CharacterManager>();
        if (characterManager != null)
        {
            contactPoint = other.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            DamageTarget(characterManager);
        }
    }

    protected virtual void DamageTarget(CharacterManager damagedTarget)
    {
        if (characterDamaged.Contains(damagedTarget))
        {
            return;
        }
        characterDamaged.Add(damagedTarget);

        TakeHealthDamageEffect damageEffect = Instantiate(CharacterEffectsManager.Instance.takeDamageEffect);
        damageEffect.physicalDamage = physicalDamage;
        damageEffect.magicDamage = magicDamage;
        damageEffect.fireDamage = fireDamage;
        damageEffect.lightningDamage = lightningDamage;
        damageEffect.holyDamage = holyDamage;
        damageEffect.contactPoint = contactPoint;

        damagedTarget.characterEffects.ProcessInstantEffects(damageEffect);
    }
}
