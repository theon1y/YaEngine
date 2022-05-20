using System.Threading.Tasks;
using Silk.NET.Windowing;
using YaEcs;

namespace YaEngine.Bootstrap.Silk
{
    public class BindSilkApplicationSystem : IInitializeSystem
    {
        public int Priority => InitializePriorities.First;
        
        private readonly IWindow window;

        public BindSilkApplicationSystem(IWindow window)
        {
            this.window = window;
        }
        
        public Task ExecuteAsync(IWorld world)
        {
            world.AddSingleton(new Application
            {
                Instance = new SilkApplication(window) 
            });
            return Task.CompletedTask;
        }
    }
}