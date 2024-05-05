using UnityEngine;

public class CharacterEffects : MonoBehaviour
{
    //Instant effects (Take dame, heal)
    //Timed effects (Poison, ...)
    //Static effects (Add/removing buff, ...)

    private IEffectable characterEffectable;

    protected virtual void Awake()
    {
        characterEffectable = GetComponent<IEffectable>();
    }

    public virtual void ProcessInstantEffects(ScriptableInstantCharacterEffect effect)
    {
        effect.ProcessEffect(characterEffectable);
    }
}
