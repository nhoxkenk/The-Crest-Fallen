using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackStabCollider : MonoBehaviour
{
    public BoxCollider Collider;
    public Transform backStabberTransform;

    private void Awake()
    {
        Collider = GetComponent<BoxCollider>();
    }
}
