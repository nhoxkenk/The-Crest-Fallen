using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    [Header("Collider")]
    protected Collider damageCollider;

    [Header("Damage")]
    public int physicalDamage;
    public int magicDamage;
    public int fireDamage;
    public int lightningDamage;
    public int holyDamage;

    [Header("Contact Point")]
    [SerializeField] protected Vector3 contactPoint;

    [Header("Character Damaged")]
    protected List<CharacterManager> characterDamaged = new List<CharacterManager>();

    [Header("Character Causing Damage")]
    public CharacterManager characterCausingDamage;

    protected virtual void Awake()
    {
        damageCollider = GetComponent<Collider>();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        CharacterManager characterEffectable = other.GetComponentInParent<CharacterManager>();
        if (characterEffectable != null)
        {
            contactPoint = other.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

            DamageTarget(characterEffectable);
        }
    }

    protected virtual void DamageTarget(CharacterManager characterEffectable)
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

        characterEffectable.characterEffects.ProcessInstantEffects(damageEffect);
    }

    public virtual void EnableDamageCollider()
    {
        damageCollider.enabled = true;
    }

    public virtual void DisableDamageCollider()
    {
        damageCollider.enabled = false;
        characterDamaged.Clear();
    }
}
