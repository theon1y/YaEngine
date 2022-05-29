using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
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
            UpdateStepRegistry updateStepRegistry,
            ILogger<World> worldLogger)
        {
            this.window = window;
            this.modelWorld = modelWorld;
            renderWorld = new World(updateStepRegistry,
                initializeRenderSystems, renderSystems, disposeRenderSystems,
                modelWorld.Components, modelWorld.Entities,
                worldLogger);
            physicsWorld = new World(updateStepRegistry,
                initializePhysicsSystems, physicsSystems, disposePhysicsSystems,
                modelWorld.Components, modelWorld.Entities,
                worldLogger);
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
            Update<Time>(modelWorld, deltaTime);
            if (IsClosing) return;
            
            Update<PhysicsTime>(physicsWorld, deltaTime);
        }

        private void RenderWorld(double deltaTime)
        {
            Update<RenderTime>(renderWorld, deltaTime);
        }

        private static void Update<TTime>(IWorld world, double deltaTime) where TTime : Time
        {
            if (world.TryGetSingleton(out TTime time))
            {
                time.DeltaTime = (float) deltaTime;
                time.TimeSinceStartup += deltaTime;
            }
            world.Update();
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