using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventory : MonoBehaviour
{
    public ConsumeItem consumeItem;

    private CharacterManager characterManager;

    protected virtual void Start()
    {
        characterManager = GetComponent<CharacterManager>();

        if (consumeItem != null)
        {
            consumeItem = Instantiate(consumeItem);
            consumeItem.characterHolding = characterManager;
        } 
    }
}
