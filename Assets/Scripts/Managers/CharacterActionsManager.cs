using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterActionsManager : Singleton<CharacterActionsManager>
{
    [Header("Weapon Item Actions")]
    public ScriptableWeaponItemAction[] actions;

    private void Start()
    {
        GeneratedActionId();
    }

    private void GeneratedActionId()
    {
        int index = 0;
        foreach (var action in actions)
        {
            action.actionId = index++;
        }
    }

    public ScriptableWeaponItemAction GetWeaponItemActionById(int id)
    {
        return actions.FirstOrDefault(action => action.actionId == id);
    }
}
