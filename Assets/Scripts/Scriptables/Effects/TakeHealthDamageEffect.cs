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
        PlaySoundFXHappenDuringAttack(characterEffectable);
        PlayDamagedAnimationBasedOnHitDirection(characterEffectable);
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

    private void PlaySoundFXHappenDuringAttack(CharacterManager characterEffectable)
    {
        AudioClip soundClip = SoundEffectsManager.Instance.hitSFX;
        characterEffectable.characterSoundEffect.PlaySoundFX(soundClip);
    }

    private void PlayDamagedAnimationBasedOnHitDirection(CharacterManager characterEffectable)
    {
        IsPoiseBroken = true;

        var characterAnimator = characterEffectable.characterAnimator;
        if ((angleHitFrom >= 145 && angleHitFrom <= 180) || (angleHitFrom <= -145 && angleHitFrom >= -180))
        {
            damagedAnimation = characterAnimator.hit_Forward_Medium_01;
        }
        else if (angleHitFrom >= -45 && angleHitFrom <= 45)
        {
            damagedAnimation = characterAnimator.hit_Backward_Medium_01;
        }
        else if (angleHitFrom >= -144 && angleHitFrom < -45)
        {
            damagedAnimation = characterAnimator.hit_Left_Medium_01;
        }
        else if (angleHitFrom > 45 && angleHitFrom <= 144)
        {
            damagedAnimation = characterAnimator.hit_Right_Medium_01;
        }

        if(IsPoiseBroken)
        {
            characterAnimator.PlayTargetActionAnimation(damagedAnimation, true);
        }
    }
}
