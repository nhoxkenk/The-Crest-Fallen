using UnityEngine;

public class CharacterEffects : MonoBehaviour
{
    //Instant effects (Take dame, heal)
    //Timed effects (Poison, ...)
    //Static effects (Add/removing buff, ...)

    private CharacterManager characterEffectable;

    [SerializeField] private VfxFactory vfxFactory;
    private IVfx bloodSplatVfx;

    protected virtual void Awake()
    {
        characterEffectable = GetComponent<CharacterManager>();
    }

    public virtual void ProcessInstantEffects(ScriptableInstantCharacterEffect effect)
    {
        effect.ProcessEffect(characterEffectable);
    }

    public virtual void PlayBloodSplatterVFX(Vector3 contactPoint)
    {
        bloodSplatVfx = vfxFactory.GetVfx(contactPoint);
    }
}
