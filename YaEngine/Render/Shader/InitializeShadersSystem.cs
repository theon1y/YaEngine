using System.Threading.Tasks;
using YaEcs;
using YaEngine.Bootstrap;

namespace YaEngine.Render
{
    public class InitializeShadersSystem : IInitializeRenderSystem
    {
        public int Priority => InitializePriorities.Third;
        
        public Task ExecuteAsync(IWorld world)
        {
            if (world.TryGetSingleton(out ShaderRegistry _)) return Task.CompletedTask;
            
            world.AddSingleton(new ShaderRegistry());
            
            return Task.CompletedTask;
        }
    }
}