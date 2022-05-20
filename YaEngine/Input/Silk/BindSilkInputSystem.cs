using System.Threading.Tasks;
using Silk.NET.Input;
using Silk.NET.Windowing;
using YaEcs;
using YaEngine.Bootstrap;

namespace YaEngine.Input.Silk
{
    public class BindSilkInputSystem : IInitializeSystem
    {
        public int Priority => InitializePriorities.First;

        private readonly IWindow window;
        
        public BindSilkInputSystem(IWindow window)
        {
            this.window = window;
        }
        
        public Task ExecuteAsync(IWorld world)
        {
            var input = window.CreateInput();
            input.Mice[0].Cursor.CursorMode = CursorMode.Raw;
            world.AddSingleton<InputContext>(new SilkInputContextComponent(input));
            return Task.CompletedTask;
        }
    }
}