using System.Threading.Tasks;
using YaEcs;
using YaEngine.Bootstrap;

namespace YaEngine.Render
{
    public class InitializeTexturesSystem : IInitializeRenderSystem
    {
        public int Priority => InitializePriorities.Third;
        
        public Task ExecuteAsync(IWorld world)
        {
            if (world.TryGetSingleton(out TextureRegistry _)) return Task.CompletedTask;

            world.AddSingleton(new TextureRegistry());
            
            return Task.CompletedTask;
        }
    }
}