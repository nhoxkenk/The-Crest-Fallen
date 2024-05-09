using UnityEngine;

public abstract class VfxFactory : ScriptableObject
{
    public abstract IVfx GetVfx(Vector3 position);
}
