using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBackStabable
{
    public BoxCollider BackStabCollider {  get; }
    public Transform BackStabberTransform { get; }

    public float BackStabberDistance();

    public bool IsBeingBackStabbed { get; set; }
}
