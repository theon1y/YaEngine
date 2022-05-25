using YaEngine.VFX.ParticleSystem.Modules.Shapes;

namespace YaEngine.VFX.ParticleSystem.Modules
{
    public class ShapeModule : IModule
    {
        public IShape Shape;
        
        public void Process(ParticleStorage particles, float deltaTime)
        {
            particles.ForEachAlive((ref Particle particle) =>
            {
                if (particle.Time > deltaTime) return;

                particle.Direction = Shape.GetDirection(particles.Random.NextSingle());
            });
        }
    }
}