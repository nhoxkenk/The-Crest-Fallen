public class BloodSplatFactory : VfxFactory
{
    public override IVfx CreateVfx()
    {
        return new BloodSplat();
    }
}
