using UnityEngine;

public class PlayerEffects : CharacterEffects
{
    [Header("For Debug purpose")]
    [SerializeField] private ScriptableInstantCharacterEffect temporaryEffect;
    [SerializeField] private bool isTriggerEffect;

    private void Update()
    {
        if (isTriggerEffect)
        {
            isTriggerEffect = false;

            ScriptableInstantCharacterEffect effect = Instantiate(temporaryEffect);
            ProcessInstantEffects(effect);
        }
    }
}
