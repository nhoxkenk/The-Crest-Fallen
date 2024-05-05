using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEffectable
{
    public bool IsAlive { get; set; }
    public void TakeInstantHealthEffect(float damage);
    public void TakeInstantStaminaEffect(float damage);
    public void ProcessInstantEffects(ScriptableInstantCharacterEffect effect);
}
