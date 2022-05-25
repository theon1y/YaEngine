using YaEcs;
using YaEngine.Render;

namespace YaEngine.VFX.ParticleSystem
{
    public class ParticleRenderer : IComponent
    {
        public ParticleStorage Particles;
        public Mesh Mesh;
    }
}