using System.Numerics;
using YaEngine.VFX.ParticleSystem.Modules.Value;

namespace YaEngine.VFX.ParticleSystem.Modules
{
    public class ScaleModule : IModule
    {
        public IValueProvider<Vector3> Provider;
        
        public void Process(ParticleStorage particles, float deltaTime)
        {
            particles.ForEachAlive((ref Particle particle) =>
            {
                particle.Scale = Provider.Get(particle.NormalizedTime);
            });
        }
    }
}