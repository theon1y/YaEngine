using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing;
using YaEcs;
using YaEngine.Bootstrap;
using YaEngine.Input;
using YaEngine.Render.OpenGL;

namespace YaEngine.ImGui
{
    public class InitializeImGuiSystem : IInitializeSystem
    {
        public int Priority => InitializePriorities.Second;
        
        private readonly IWindow window;
        private readonly List<IImGuiSystem> imGuiSystems;

        public InitializeImGuiSystem(IWindow window, IEnumerable<IImGuiSystem> imGuiSystems)
        {
            this.window = window;
            this.imGuiSystems = imGuiSystems.ToList();
        }
        
        public Task ExecuteAsync(IWorld world)
        {
            if (!world.TryGetSingleton(out RenderApi api)) return Task.CompletedTask;
            if (!world.TryGetSingleton(out InputContext input)) return Task.CompletedTask;

            var controller = new ImGuiController(api.Value, window, input.Instance);
            window.FramebufferResize += api.Value.Viewport;
            
            world.AddSingleton(new ImGui { Controller = controller });
            world.AddSingleton(new GuiRegistry(imGuiSystems));
            return Task.CompletedTask;
        }
    }
}