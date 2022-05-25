using System.Numerics;
using YaEngine.VFX.ParticleSystem.Modules.Value;

namespace YaEngine.VFX.ParticleSystem.Modules
{
    public class RotateModule : IModule
    {
        public IValueProvider<Quaternion> Provider;
        
        public void Process(ParticleStorage particles, float deltaTime)
        {
            particles.ForEachAlive((ref Particle particle) =>
            {
                particle.Rotation = Provider.Get(particle.NormalizedTime);
            });
        }
    }
}