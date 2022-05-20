using System.Threading.Tasks;
using YaEcs;
using YaEngine.Bootstrap;

namespace YaEngine.Render
{
    public class InitializeTexturesSystem : IInitializeSystem
    {
        public int Priority => InitializePriorities.Third;
        
        public Task ExecuteAsync(IWorld world)
        {
            world.AddSingleton(new TextureRegistry());
            
            return Task.CompletedTask;
        }
    }
}