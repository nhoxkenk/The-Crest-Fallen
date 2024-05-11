using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VfxBloodSplat : MonoBehaviour, IVfx
{
    private ParticleSystem particleSystemVfx;

    public void InitializeVisualFX()
    {
        particleSystemVfx = GetComponent<ParticleSystem>();
        particleSystemVfx?.Stop();
        particleSystemVfx?.Play();
    }
}
