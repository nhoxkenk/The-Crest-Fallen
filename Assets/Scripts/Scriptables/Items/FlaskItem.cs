using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Flask Item")]
public class FlaskItem : ConsumeItem
{
    [Header("Flask Type")]
    public bool estusFlask;
    public bool ashenFlask;

    [Header("Recovery Amount")]
    public float recoveryAmount;

    public RestoreHealthEffect itemEffect;

    public override void UsingConsumeItem()
    {
        itemEffect = Instantiate(CharacterEffectsManager.Instance.restoreHealthEffect);
        itemEffect.consumeAnimationName = "Potion_Drink";
        itemEffect.value = recoveryAmount;

        characterHolding.characterEffects.instantiatedFXModel = Instantiate(itemModel, PlayerManager.Instance.playerEquipment.rightHandSlot.transform);
        PlayerManager.Instance.playerEquipment.rightHandSlot.UnloadWeapon();

        itemEffect.ProcessEffect(characterHolding);

        base.UsingConsumeItem();

        PlayerUI.Instance.playerUIHud.HandleConsumeItemChanged(this);
    }
}
