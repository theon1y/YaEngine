using System.Threading.Tasks;
using YaEcs;
using YaEngine.Bootstrap;

namespace YaEngine.Render
{
    public class InitializeBuffersSystem : IInitializeRenderSystem
    {
        public int Priority => InitializePriorities.First;
        
        public Task ExecuteAsync(IWorld world)
        {
            if (world.TryGetSingleton(out RenderBuffers _)) return Task.CompletedTask;
            
            world.AddSingleton(new RenderBuffers());
            return Task.CompletedTask;
        }
    }
}