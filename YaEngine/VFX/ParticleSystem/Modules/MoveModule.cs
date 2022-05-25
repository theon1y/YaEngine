namespace YaEngine.VFX.ParticleSystem.Modules
{
    public class MoveModule : IModule
    {
        public void Process(ParticleStorage particles, float deltaTime)
        {
            particles.ForEachAlive((ref Particle particle) =>
            {
                particle.Position += particle.Direction * particle.Speed * deltaTime;
            });
        }
    }
}