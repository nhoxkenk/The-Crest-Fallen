using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Factory/Vfx Factory/Blood Splat Vfx")]
public class BloodSplatFactory : VfxFactory
{
    [SerializeField] private VfxBloodSplat vfxbloodSplatPrefab;

    public override IVfx GetVfx(Vector3 position)
    {
        GameObject instance = Object.Instantiate(vfxbloodSplatPrefab.gameObject, position, Quaternion.identity);
        VfxBloodSplat newVfx = instance.GetComponent<VfxBloodSplat>();

        newVfx.InitializeVfx();

        return newVfx;
    }
}
