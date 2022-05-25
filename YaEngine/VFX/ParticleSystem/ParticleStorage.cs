using System;

namespace YaEngine.VFX.ParticleSystem
{
    public class ParticleStorage
    {
        public readonly Particle[] Particles;
        public Random Random = new();

        public ParticleStorage(int maxParticles)
        {
            Particles = new Particle[maxParticles];
        }

        public ref Particle this[int i] => ref Particles[i];
        
        public int UsedParticles;
        public float LastSpawnTime;
        public float Lifetime;
        public bool IsPlaying;
    }
}