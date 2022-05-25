namespace YaEngine.VFX.ParticleSystem.Modules
{
    public class LifetimeModule : IModule
    {
        public void Process(ParticleStorage particles, float deltaTime)
        {
            particles.ForEachAlive((ref Particle particle) =>
            {
                particle.Time += deltaTime;
                particle.NormalizedTime += deltaTime / particle.Duration;
                if (particle.Time >= particle.Duration)
                {
                    particle.IsAlive = false;
                    particle.Scale = default;
                }
            });
        }
    }
}