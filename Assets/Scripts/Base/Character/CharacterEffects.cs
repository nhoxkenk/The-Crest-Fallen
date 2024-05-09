using UnityEngine;

public class CharacterEffects : MonoBehaviour
{
    //Instant effects (Take dame, heal)
    //Timed effects (Poison, ...)
    //Static effects (Add/removing buff, ...)

    private CharacterManager characterEffectable;

    [SerializeField] private VfxFactory vfxFactory;

    protected virtual void Awake()
    {
        characterEffectable = GetComponent<CharacterManager>();
    }

    protected virtual void Start()
    {
        
    }

    public virtual void ProcessInstantEffects(ScriptableInstantCharacterEffect effect)
    {
        effect.ProcessEffect(characterEffectable);
    }

    public virtual void PlayBloodSplatterVFX(Vector3 contactPoint)
    {
        vfxFactory.GetVfx(contactPoint);
    }
}
