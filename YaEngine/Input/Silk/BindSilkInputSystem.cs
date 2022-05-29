using System.Threading.Tasks;
using Silk.NET.Input;
using Silk.NET.Windowing;
using YaEcs;
using YaEngine.Bootstrap;
using YaEngine.Model;

namespace YaEngine.Input.Silk
{
    public class BindSilkInputSystem : IInitializeModelSystem
    {
        public int Priority => InitializePriorities.First;

        private readonly IWindow window;
        
        public BindSilkInputSystem(IWindow window)
        {
            this.window = window;
        }
        
        public Task ExecuteAsync(IWorld world)
        {
            if (world.TryGetSingleton(out InputContext _)) return Task.CompletedTask;
            
            var input = window.CreateInput();
            world.AddSingleton<InputContext>(new SilkInputContextComponent(input));
            return Task.CompletedTask;
        }
    }
}