using System.Threading.Tasks;
using YaEcs;
using YaEngine.Bootstrap;

namespace YaEngine.Render
{
    public class InitializeShadersSystem : IInitializeSystem
    {
        public int Priority => InitializePriorities.Third;
        
        public Task ExecuteAsync(IWorld world)
        {
            world.AddSingleton(new ShaderRegistry());
            
            return Task.CompletedTask;
        }
    }
}