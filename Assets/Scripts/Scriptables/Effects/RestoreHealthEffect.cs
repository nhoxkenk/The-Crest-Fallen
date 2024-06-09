using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

[CreateAssetMenu(menuName = "Character Effects/Instant Effects/Restore Health Effect")]
public class RestoreHealthEffect : ScriptableInstantCharacterEffect
{
    [Header("Animation name")]
    public string consumeAnimationName;

    public float value;

    private void OnEnable()
    {
        OnProcessEffect += PlayConsumeFlaskAnimation;
        OnProcessEffect += RestoreHealth;
    }

    private void OnDisable()
    {
        OnProcessEffect -= PlayConsumeFlaskAnimation;
        OnProcessEffect -= RestoreHealth;
    }

    public override void ProcessEffect(CharacterManager characterEffectable)
    {
        base.ProcessEffect(characterEffectable);
    }

    private void PlayConsumeFlaskAnimation(CharacterManager characterEffectable)
    {
        if (characterEffectable.characterInventory.consumeItem.quantity > 0)
        {
            characterEffectable.characterAnimator.PlayTargetActionAnimation(consumeAnimationName, false, false, true, true);
        }
        else
        {
            characterEffectable.characterAnimator.PlayTargetActionAnimation("Potion_Empty", false, false, true, true);
        }
    }

    private void RestoreHealth(CharacterManager characterEffectable)
    {
        if (characterEffectable.characterInventory.consumeItem.quantity > 0)
        {
            characterEffectable.characterStat.CurrentHealth += value;
        }   
    }
}
