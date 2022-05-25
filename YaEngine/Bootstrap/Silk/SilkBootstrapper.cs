using System.Collections.Generic;
using Silk.NET.Windowing;
using YaEcs;
using YaEngine.Core;
using YaEngine.Render;

namespace YaEngine.Bootstrap
{
    public class SilkBootstrapper
    {
        private readonly IWindow window;
        private readonly IWorld modelWorld;
        private readonly IWorld renderWorld;

        public SilkBootstrapper(IWindow window, IWorld modelWorld,
            IEnumerable<IInitializeRenderSystem> initializeRenderSystems,
            IEnumerable<IRenderSystem> renderSystems,
            IEnumerable<IDisposeRenderSystem> disposeRenderSystems,
            UpdateStepRegistry updateStepRegistry)
        {
            this.window = window;
            this.modelWorld = modelWorld;
            renderWorld = new World(updateStepRegistry,
                initializeRenderSystems, renderSystems, disposeRenderSystems,
                modelWorld.Components, modelWorld.Entities);
            window.Load += InitializeWorld;
            window.Update += UpdateWorld;
            window.Render += RenderWorld;
            window.Closing += DisposeWorld;
        }

        public void Run()
        {
            window.Run();
        }

        private void InitializeWorld()
        {
            modelWorld.InitializeAsync().GetAwaiter().GetResult();
            renderWorld.InitializeAsync().GetAwaiter().GetResult();
        }

        private void UpdateWorld(double deltaTime)
        {
            if (modelWorld.TryGetSingleton(out Time time))
            {
                time.DeltaTime = (float) deltaTime;
                time.TimeSinceStartup += deltaTime;
            }
            
            modelWorld.Update();
        }

        private void RenderWorld(double deltaTime)
        {
            if (modelWorld.TryGetSingleton(out RenderTime time))
            {
                time.DeltaTime = (float) deltaTime;
            }

            renderWorld.Update();
        }
        
        private void DisposeWorld()
        {
            modelWorld.DisposeAsync().GetAwaiter().GetResult();
            renderWorld.DisposeAsync().GetAwaiter().GetResult();
        }
    }
}