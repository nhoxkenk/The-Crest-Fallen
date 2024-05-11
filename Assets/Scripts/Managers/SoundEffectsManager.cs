using UnityEngine;

public class SoundEffectsManager : Singleton<SoundEffectsManager>
{
    [Header("Action Sounds")]
    public AudioClip rollSFX;
    public AudioClip hitSFX;

    [Header("Damage Sounds")]
    public AudioClip[] physicalDamageSFX;


    public AudioClip PlayRandomSwingSoundEffect()
    {
        int index = Random.Range(0, physicalDamageSFX.Length);
        return physicalDamageSFX[index];
    }
}
