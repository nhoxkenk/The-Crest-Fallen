using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Melee Weapon Item")]
public class MeleeWeaponItem : WeaponItem
{
    public ScriptableInstantCharacterEffect[] effectHoldByWeapon;

    public bool IsWeaponHasEffect()
    {
        return effectHoldByWeapon.Length > 0;
    }

    public ScriptableInstantCharacterEffect[] GetEnhanceStatEffect()
    {
        var enhanceStatEffect = new List<ScriptableInstantCharacterEffect>();
        foreach (var effect in effectHoldByWeapon)
        {
            if(effect != null && effect.effectType == EffectType.EffectStat)
            {
                enhanceStatEffect.Add(effect);
            }
        }
        return enhanceStatEffect.ToArray();
    }
}
