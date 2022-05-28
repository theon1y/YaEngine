using System;
using System.Collections.Generic;
using Silk.NET.Windowing;
using YaEcs;
using YaEngine.Core;
using YaEngine.Physics;
using YaEngine.Render;

namespace YaEngine.Bootstrap
{
    public class SilkBootstrapper
    {
        private readonly IWindow window;
        private readonly IWorld modelWorld;
        private readonly IWorld renderWorld;
        private readonly IWorld physicsWorld;
        private bool IsClosing;

        public SilkBootstrapper(IWindow window, IWorld modelWorld,
            IEnumerable<IInitializeRenderSystem> initializeRenderSystems,
            IEnumerable<IRenderSystem> renderSystems,
            IEnumerable<IDisposeRenderSystem> disposeRenderSystems,
            IEnumerable<IInitializePhysicsSystem> initializePhysicsSystems,
            IEnumerable<IPhysicsSystem> physicsSystems,
            IEnumerable<IDisposePhysicsSystem> disposePhysicsSystems,
            UpdateStepRegistry updateStepRegistry)
        {
            this.window = window;
            this.modelWorld = modelWorld;
            renderWorld = new World(updateStepRegistry,
                initializeRenderSystems, renderSystems, disposeRenderSystems,
                modelWorld.Components, modelWorld.Entities);
            physicsWorld = new World(updateStepRegistry,
                initializePhysicsSystems, physicsSystems, disposePhysicsSystems,
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
            physicsWorld.InitializeAsync().GetAwaiter().GetResult();
        }

        private void UpdateWorld(double deltaTime)
        {
            try
            {
                UpdateWorlds((float) deltaTime);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void UpdateWorlds(float deltaTime)
        {
            if (modelWorld.TryGetSingleton(out Time time))
            {
                time.DeltaTime = deltaTime;
                time.TimeSinceStartup += deltaTime;
            }

            modelWorld.Update();
            if (IsClosing) return;

            if (physicsWorld.TryGetSingleton(out PhysicsTime physicsTime))
            {
                physicsTime.DeltaTime = deltaTime;
            }
            physicsWorld.Update();
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
            IsClosing = true;
            physicsWorld.DisposeAsync().GetAwaiter().GetResult();
            renderWorld.DisposeAsync().GetAwaiter().GetResult();
            modelWorld.DisposeAsync().GetAwaiter().GetResult();
        }
    }
}