using System.Threading.Tasks;
using YaEcs;
using YaEngine.Bootstrap;

namespace YaEngine.Physics
{
    public class InitializePhysicsSystem : IInitializePhysicsSystem
    {
        public int Priority => InitializePriorities.First;
        
        public Task ExecuteAsync(IWorld world)
        {
            if (world.TryGetSingleton(out PhysicsTime _)) return Task.CompletedTask;
            
            world.AddSingleton(new PhysicsTime());
            return Task.CompletedTask;
        }
    }
}