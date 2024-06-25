using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIUndeadCombat : AICharacterCombat, IBackStabable
{
    [Header("Character Damage Collider")]
    [SerializeField] private DamageCollider leftHandCollider;
    [SerializeField] private DamageCollider rightHandCollider;

    [Header("Damage")]
    [SerializeField] private int baseDamage = 25;
    [SerializeField] private float attack01DamageModifier = 1.1f;
    [SerializeField] private float attack02DamageModifier = 1.15f;

    [Header("Back Stab")]
    [SerializeField] private BackStabCollider backStabCollider;
    public BoxCollider BackStabCollider => backStabCollider.Collider;

    public Transform BackStabberTransform => backStabCollider.backStabberTransform;

    private bool isBeingBackStabbed = false;
    public bool IsBeingBackStabbed { get => isBeingBackStabbed; set => isBeingBackStabbed = value; }

    public event Action BackStabed = delegate { };  

    public override void ApplyAttack01DamageModifier()
    {
        base.ApplyAttack01DamageModifier();
        rightHandCollider.physicalDamage = (int)(baseDamage * attack01DamageModifier);
        leftHandCollider.physicalDamage = (int)(baseDamage * attack01DamageModifier);
    }

    public override void ApplyAttack02DamageModifier()
    {
        base.ApplyAttack02DamageModifier();
        rightHandCollider.physicalDamage = (int)(baseDamage * attack02DamageModifier);
        leftHandCollider.physicalDamage = (int)(baseDamage * attack02DamageModifier);
    }

    public void OpenRightHandCollider()
    {
        rightHandCollider.EnableDamageCollider();
    }

    public void OpenLeftHandCollider()
    {
        leftHandCollider.EnableDamageCollider();
    }

    public void CloseRightHandCollider()
    {
        rightHandCollider?.DisableDamageCollider();
    }

    public void CloseLeftHandCollider()
    {
        leftHandCollider?.DisableDamageCollider();
    }

    public float BackStabberDistance()
    {
        return Vector3.Distance(transform.position, BackStabberTransform.transform.position);
    }

    public void ResetIsBeingStabed()
    {
        isBeingBackStabbed = false;
    }
}
