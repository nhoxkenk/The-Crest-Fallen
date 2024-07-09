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

    private void OnEnable()
    {
        OnProcessEffect += CalculateDamage;
        OnProcessEffect += PlayVfxHappenDuringAttack;
        OnProcessEffect += PlaySoundFXHappenDuringAttack;
        OnProcessEffect += PlayDamagedAnimationBasedOnHitDirection;
    }

    private void OnDisable()
    {
        OnProcessEffect -= CalculateDamage;
        OnProcessEffect -= PlayVfxHappenDuringAttack;
        OnProcessEffect -= PlaySoundFXHappenDuringAttack;
        OnProcessEffect -= PlayDamagedAnimationBasedOnHitDirection;
    }

    public override void ProcessEffect(CharacterManager characterEffectable)
    {
        if(!characterEffectable.IsAlive || characterEffectable.IsInvulnerable)
        {
            return;
        }

        base.ProcessEffect(characterEffectable);
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

        characterEffectable.characterStat.CurrentHealth -= finalDamageDealt;
    }

    private void PlayVfxHappenDuringAttack(CharacterManager characterEffectable)
    {
        characterEffectable.characterEffects.PlayBloodSplatterVFX(contactPoint);
    }

    private void PlaySoundFXHappenDuringAttack(CharacterManager characterEffectable)
    {
        characterEffectable.characterSoundEffect.PlayDamagedGruntSoundFX();
    }

    private void PlayDamagedAnimationBasedOnHitDirection(CharacterManager characterEffectable)
    {
        IsPoiseBroken = true;

        if(!characterEffectable.IsAlive)
        {
            return;
        }

        if(characterEffectable.TryGetComponent<IBackStabable>(out IBackStabable character))
        {
            if (character.IsBeingBackStabbed)
            {
                return;
            }
        }

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
