using System.Threading.Tasks;
using YaEcs;
using YaEngine.Bootstrap;

namespace YaEngine.Core
{
    public class InitializeCameraRegistrySystem : IInitializeSystem
    {
        public int Priority => InitializePriorities.First;
        
        public Task ExecuteAsync(IWorld world)
        {
            world.AddSingleton(new CameraRegistry());
            return Task.CompletedTask;
        }
    }
}