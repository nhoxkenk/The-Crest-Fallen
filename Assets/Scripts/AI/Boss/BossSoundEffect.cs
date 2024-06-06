using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSoundEffect : CharacterSoundEffect
{
    [Header("Weapon Whooshes")]
    [SerializeField] protected AudioClip[] weaponWhooshes;

    public void PlayWeaponWhooshesSoundFX()
    {
        PlaySoundFX(SoundEffectsManager.Instance.PlayRandomSoundFXFromArray(weaponWhooshes));
    }
}
