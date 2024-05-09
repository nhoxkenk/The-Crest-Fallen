using UnityEngine;

public interface IVfx
{
    public GameObject VFX { get; set; }
    public void PlayVfx(Vector3 contactPoint);

    static IVfx CreateDefault()
    {
        return new BloodSplat();
    }
}
