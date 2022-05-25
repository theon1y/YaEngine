using System;
using System.Numerics;
using YaEngine.Core;

namespace YaEngine.VFX.ParticleSystem.Modules
{
    public class EmissionModule : IModule
    {
        public float Rate;
        public Burst[]? Bursts;
        public float Duration;
        public bool IsLooping;
        public Vector2 ParticleLifetime;
        public Vector2 ParticleSpeed;
        
        public void Process(ParticleStorage particles, float deltaTime)
        {
            particles.Lifetime += deltaTime;
            if (particles.Lifetime >= Duration)
            {
                if (IsLooping)
                {
                    particles.LastSpawnTime = 0;
                    particles.Lifetime = deltaTime;
                }
                else
                {
                    particles.IsPlaying = false;   
                }
                return;
            }

            var rateSpawns = (int)((particles.Lifetime - particles.LastSpawnTime) * Rate);
            if (rateSpawns > 0)
            {
                particles.LastSpawnTime = particles.Lifetime;
            }
            var burstSpawns = GetBurstSpawns(particles.Lifetime - deltaTime, particles.Lifetime);
            var totalSpawns = rateSpawns + burstSpawns;

            for (var i = 0; totalSpawns > 0 && i < particles.Particles.Length; ++i)
            {
                ref var particle = ref particles.Particles[i];
                if (particle.IsAlive) continue;
                
                SpawnParticle(ref particle, deltaTime, particles.Random);
                if (i >= particles.UsedParticles) ++particles.UsedParticles;
                --totalSpawns;
            }

            for (var i = 0; totalSpawns > 0 && i < particles.Particles.Length; ++i)
            {
                ref var particle = ref particles.Particles[i];
                SpawnParticle(ref particle, deltaTime, particles.Random);
                --totalSpawns;
            }
        }

        private void SpawnParticle(ref Particle particle, float deltaTime, Random random)
        {
            particle.Reset();
            particle.Rotation = Quaternion.Identity;
            particle.Time = deltaTime;
            particle.Duration = MathUtils.Interpolate(ParticleLifetime.X, ParticleLifetime.Y, random.NextSingle());
            particle.NormalizedTime = deltaTime / particle.Duration;
            particle.Speed = MathUtils.Interpolate(ParticleSpeed.X, ParticleSpeed.Y, random.NextSingle());
            particle.IsAlive = true;
        }

        private int GetBurstSpawns(float fromTime, float toTime)
        {
            if (Bursts == null) return 0;

            var result = 0;
            
            foreach (var burst in Bursts)
            {
                if (burst.Time >= fromTime && burst.Time <= toTime)
                {
                    result += burst.Count;
                }
            }

            return result;
        }
    }
}