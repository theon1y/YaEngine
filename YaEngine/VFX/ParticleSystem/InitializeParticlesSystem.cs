using System.Linq;
using System.Threading.Tasks;
using YaEcs;
using YaEngine.Bootstrap;
using YaEngine.Render;

namespace YaEngine.VFX.ParticleSystem
{
    public class InitializeParticlesSystem : IInitializeRenderSystem
    {
        public int Priority => InitializePriorities.Third;
        
        public Task ExecuteAsync(IWorld world)
        {
            world.ForEach((Entity entity, ParticleEffect effect) =>
            {
                var particleSystemMesh = BuildParticleSystemMesh(effect.MaxParticles);
                var rendererInitializer = new RendererInitializer
                    {
                        Material = effect.Material,
                        Mesh = effect.Mesh,
                        InstanceData = particleSystemMesh,
                        CullFace = false
                    };
                world.AddComponent(entity, rendererInitializer);
                
                var particleSystemStorage = new ParticleStorage(effect.MaxParticles);
                var particleRenderer = new ParticleRenderer
                {
                    Mesh = particleSystemMesh, Particles = particleSystemStorage
                };
                world.AddComponent(entity, particleRenderer);
            });
            
            return Task.CompletedTask;
        }

        private static Mesh BuildParticleSystemMesh(int maxParticles)
        {
            return new()
            {
                Vertices = new float[ParticleAttributes.Size * maxParticles],
                Attributes = ParticleAttributes.Attributes.ToList(),
                VertexSize = ParticleAttributes.Size
            };
        }
    }
}