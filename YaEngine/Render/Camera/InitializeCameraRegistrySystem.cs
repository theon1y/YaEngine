using System.Threading.Tasks;
using YaEcs;
using YaEngine.Bootstrap;

namespace YaEngine.Render
{
    public class InitializeCameraRegistrySystem : IInitializeRenderSystem
    {
        public int Priority => InitializePriorities.First;
        
        public Task ExecuteAsync(IWorld world)
        {
            if (world.TryGetSingleton(out CameraRegistry _)) return Task.CompletedTask;
            
            world.AddSingleton(new CameraRegistry());
            return Task.CompletedTask;
        }
    }
}