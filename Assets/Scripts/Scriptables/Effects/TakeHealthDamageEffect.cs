using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Health Damage Effect")]
public class TakeHealthDamageEffect : ScriptableInstantCharacterEffect
{
    [Header("Character Causing Damage")]
    [SerializeField] private CharacterManager characterCausingDamageManager;

    [Header("Damage Attributes")]
    public float physicalDamage;
    public float magicDamage;
    public float fireDamage;
    public float lightningDamage;
    public float holyDamage;

    [Header("Final Damage")]
    [SerializeField] private float finalDamageDealt;

    [Header("Poise")]
    public float poiseDamage = 0;
    public bool IsPoiseBroken { get; set; } = false;

    [Header("Animation")]
    public bool playDamagedAnimation = true; 
    public bool manuallySelectDamagedAnimation = false;
    public string damagedAnimation;

    [Header("Direction Damaged Taken")]
    public float angleHitFrom;
    public Vector3 contactPoint;

    public override void ProcessEffect(CharacterManager characterEffectable)
    {
        base.ProcessEffect(characterEffectable);

        if(!characterEffectable.IsAlive)
        {
            return;
        }

        CalculateDamage(characterEffectable);
        PlayVfxHappenDuringAttack(characterEffectable);
    }

    private void CalculateDamage(CharacterManager characterEffectable)
    {
        if(characterCausingDamageManager != null)
        {
            //Check for damage modifiers
        }

        finalDamageDealt = Mathf.RoundToInt(physicalDamage + magicDamage + fireDamage + lightningDamage + holyDamage);

        if(finalDamageDealt <= 0)
        {
            finalDamageDealt = 1;
        }

        characterEffectable.TakeInstantHealthEffect(finalDamageDealt);
    }

    private void PlayVfxHappenDuringAttack(CharacterManager characterEffectable)
    {
        characterEffectable.characterEffects.PlayBloodSplatterVFX(contactPoint);
    }
}
