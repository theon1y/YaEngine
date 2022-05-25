using System.Runtime.CompilerServices;

namespace YaEngine.VFX.ParticleSystem.Modules
{
    public static class ModuleExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEachAlive(this ParticleStorage storage, UpdateParticle action)
        {
            for (var i = 0; i < storage.UsedParticles; ++i)
            {
                ref var particle = ref storage[i];
                if (!particle.IsAlive) continue;

                action(ref particle);
            }
        }
    }
    
    public delegate void UpdateParticle(ref Particle particle);
}