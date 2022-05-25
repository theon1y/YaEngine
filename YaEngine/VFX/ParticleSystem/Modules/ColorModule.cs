using System.Numerics;
using YaEngine.VFX.ParticleSystem.Modules.Value;

namespace YaEngine.VFX.ParticleSystem.Modules
{
    public class ColorModule : IModule
    {
        public IValueProvider<Vector4> Provider;
        
        public void Process(ParticleStorage particles, float deltaTime)
        {
            particles.ForEachAlive((ref Particle particle) =>
            {
                particle.Color = Provider.Get(particle.NormalizedTime);
            });
        }
    }
}