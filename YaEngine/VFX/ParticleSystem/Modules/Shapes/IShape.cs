using System.Numerics;

namespace YaEngine.VFX.ParticleSystem.Modules.Shapes
{
    public interface IShape
    {
        Vector3 GetDirection(float t);
    }
}