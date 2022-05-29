using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing;
using YaEcs;
using YaEngine.Bootstrap;
using YaEngine.Input;
using YaEngine.Render;
using YaEngine.Render.OpenGL;

namespace YaEngine.ImGui
{
    public class InitializeImGuiSystem : IInitializeRenderSystem
    {
        public int Priority => InitializePriorities.Second;
        
        private readonly IWindow window;

        public InitializeImGuiSystem(IWindow window)
        {
            this.window = window;
        }
        
        public Task ExecuteAsync(IWorld world)
        {
            if (world.TryGetSingleton(out GuiRegistry _)) return Task.CompletedTask;
            if (!world.TryGetSingleton(out RenderApi api)) return Task.CompletedTask;
            if (!world.TryGetSingleton(out InputContext input)) return Task.CompletedTask;
            if (api is not GlRenderApi { Gl: var gl })
            {
                throw new ArgumentException($"Unsupported render api {api}");
            }

            var controller = new ImGuiController(gl, window, input.Instance);
            window.FramebufferResize += gl.Viewport;
            
            world.AddSingleton(new ImGui { Controller = controller });
            world.AddSingleton(new GuiRegistry());
            return Task.CompletedTask;
        }
    }
}