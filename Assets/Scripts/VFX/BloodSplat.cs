using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSplat : MonoBehaviour, IVfx
{
    [SerializeField] private GameObject bloodSplatVFX;

    public GameObject VFX { get { return bloodSplatVFX; } set => bloodSplatVFX = value; }

    public void PlayVfx(Vector3 contactPoint)
    {
        if(bloodSplatVFX != null)
        {
            Instantiate(bloodSplatVFX, contactPoint, Quaternion.identity);
        }
        else
        {
            Instantiate(CharacterEffectsManager.Instance.bloodSplatVFX, contactPoint, Quaternion.identity);
        }
    }
}
