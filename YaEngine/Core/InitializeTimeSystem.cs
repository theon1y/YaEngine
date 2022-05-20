using System.Threading.Tasks;
using YaEcs;
using YaEngine.Bootstrap;

namespace YaEngine.Core
{
    public class InitializeTimeSystem : IInitializeSystem
    {
        public int Priority => InitializePriorities.First;
        
        public Task ExecuteAsync(IWorld world)
        {
            world.AddSingleton(new Time());
            return Task.CompletedTask;
        }
    }
}