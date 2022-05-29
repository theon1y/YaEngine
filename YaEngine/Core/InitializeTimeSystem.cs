using System.Threading.Tasks;
using YaEcs;
using YaEngine.Bootstrap;
using YaEngine.Model;

namespace YaEngine.Core
{
    public class InitializeTimeSystem : IInitializeModelSystem
    {
        public int Priority => InitializePriorities.First;
        
        public Task ExecuteAsync(IWorld world)
        {
            if (world.TryGetSingleton(out Time _)) return Task.CompletedTask;
            
            world.AddSingleton(new Time());
            return Task.CompletedTask;
        }
    }
}