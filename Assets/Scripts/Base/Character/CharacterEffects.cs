using UnityEngine;

public class CharacterEffects : MonoBehaviour
{
    //Instant effects (Take dame, heal)
    //Timed effects (Poison, ...)
    //Static effects (Add/removing buff, ...)

    private CharacterManager characterManager;

    protected virtual void Awake()
    {
        characterManager = GetComponent<CharacterManager>();
    }

    public virtual void ProcessInstantEffects(ScriptableInstantCharacterEffect effect)
    {
        effect.ProcessEffect(characterManager);
    }
}
