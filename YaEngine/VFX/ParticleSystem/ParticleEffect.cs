using System.Collections.Generic;
using YaEcs;
using YaEngine.Render;

namespace YaEngine.VFX.ParticleSystem
{
    public class ParticleEffect : IComponent
    {
        public List<IModule> Modules;
        public MaterialInitializer Material;
        public Mesh Mesh;
        public int MaxParticles;
    }
}