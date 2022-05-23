﻿using System.Drawing;
using System.Threading.Tasks;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using YaEcs;
using YaEngine.Bootstrap;

namespace YaEngine.Render.OpenGL
{
    public class BindSilkRenderApiSystem : IInitializeSystem
    {
        public int Priority => InitializePriorities.First;

        private readonly IWindow window;
        
        public BindSilkRenderApiSystem(IWindow window)
        {
            this.window = window;
        }
        
        public Task ExecuteAsync(IWorld world)
        {
            world.AddSingleton<RenderApi>(new GlRenderApi(GL.GetApi(window), Color.Gray));
            return Task.CompletedTask;
        }
    }
}