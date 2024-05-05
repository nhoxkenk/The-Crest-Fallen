using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    protected List<IEffectable> characterDamaged = new List<IEffectable>();

    private void OnTriggerEnter(Collider other)
    {
        IEffectable characterEffectable = other.GetComponent<IEffectable>();
        if (characterEffectable != null)
        {
            contactPoint = other.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            DamageTarget(characterEffectable);
        }
    }

    protected virtual void DamageTarget(IEffectable characterEffectable)
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

        characterEffectable.ProcessInstantEffects(damageEffect);
    }
}
