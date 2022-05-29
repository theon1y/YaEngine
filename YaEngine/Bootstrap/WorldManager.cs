using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using YaEcs;
using YaEngine.Core;
using YaEngine.Model;
using YaEngine.Physics;
using YaEngine.Render;
using YaEngine.SceneManagement;

namespace YaEngine.Bootstrap
{
    public class WorldManager : IAsyncDisposable
    {
        public readonly IEntities Entities;
        public readonly IComponents Components;

        public readonly IReadOnlyList<IInitializeSystem> InitializeSystems;
        public readonly IReadOnlyList<IUpdateSystem> UpdateSystems;
        public readonly IReadOnlyList<IDisposeSystem> DisposeSystems;
        public readonly IReadOnlyList<UpdateStep> UpdateSteps;

        public IWorld? Model;
        public IWorld? Physics;
        public IWorld? Render;

        private readonly ILoggerFactory loggerFactory;
        private readonly ILogger<WorldManager> logger;
        private readonly ConcurrentQueue<Action> updateActionQueue;

        public WorldManager(IEnumerable<IInitializeSystem> initializeSystems,
            IEnumerable<IUpdateSystem> updateSystems,
            IEnumerable<IDisposeSystem> disposeSystems,
            IEnumerable<UpdateStep> updateSteps,
            ILoggerFactory loggerFactory)
        {
            this.loggerFactory = loggerFactory;
            logger = loggerFactory.CreateLogger<WorldManager>();
            updateActionQueue = new ConcurrentQueue<Action>();

            UpdateSteps = updateSteps.ToList();
            InitializeSystems = initializeSystems.ToList();
            UpdateSystems = updateSystems.ToList();
            DisposeSystems = disposeSystems.ToList();

            Entities = new Entities();
            Components = new Components();
        }

        public async Task LoadAsync(SceneSystems scene)
        {
            var initializeSystems = InitializeSystems.Concat(scene.InitializeSystems).ToList();
            var updateSystems = UpdateSystems.Concat(scene.UpdateSystems).ToList();
            var disposeSystems = DisposeSystems.Concat(scene.DisposeSystems).ToList();

            Model = CreateWorld<IModelMarker>(initializeSystems, updateSystems, disposeSystems);
            Render = CreateWorld<IRenderMarker>(initializeSystems, updateSystems, disposeSystems);
            Physics = CreateWorld<IPhysicsMarker>(initializeSystems, updateSystems, disposeSystems);

            await InitializeWorlds();
        }

        private async Task InitializeWorlds()
        {
            if (Model != null) Model.InitializeAsync().GetAwaiter().GetResult();
            if (Render != null) Render.InitializeAsync().GetAwaiter().GetResult();
            if (Physics != null) Physics.InitializeAsync().GetAwaiter().GetResult();
        }

        public void UpdateWorlds(float deltaTime)
        {
            while (updateActionQueue.TryDequeue(out var action))
            {
                try
                {
                    action();
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Could not execute an action from the action queue");
                }
            }
            Update<Time>(Model, deltaTime);
            Update<PhysicsTime>(Physics, deltaTime);
        }

        public void RenderWorlds(float deltaTime)
        {
            Update<RenderTime>(Render, deltaTime);
        }

        public async ValueTask DisposeAsync()
        {
            if (Physics != null)
            {
                Physics.DisposeAsync().GetAwaiter().GetResult();
                Physics = null;
            }

            if (Render != null)
            {
                Render.DisposeAsync().GetAwaiter().GetResult();
                Render = null;
            }

            if (Model != null)
            {
                Model.DisposeAsync().GetAwaiter().GetResult();
                Model = null;
            }
        }

        public void OnNextUpdate(Action action)
        {
            updateActionQueue.Enqueue(action);
        }

        private IWorld CreateWorld<TMarker>(IEnumerable<IInitializeSystem> initializeSystems,
            IEnumerable<IUpdateSystem> updateSystems,
            IEnumerable<IDisposeSystem> disposeSystems)
        {
            var markedUpdateSteps = UpdateSteps.Where(x => x is TMarker);
            var updateStepRegistry = new UpdateStepRegistry(markedUpdateSteps);
            var markedInitializeSystems = initializeSystems
                .Where(x => x is TMarker)
                .ToList();
            var markedUpdateSystems = updateSystems
                .Where(x => x is TMarker)
                .ToList();
            var markedDisposeSystems = disposeSystems
                .Where(x => x is TMarker)
                .ToList();
            var worldLogger = loggerFactory.CreateLogger<World>();
            return new World(updateStepRegistry, markedInitializeSystems, markedUpdateSystems, markedDisposeSystems,
                Components, Entities, worldLogger);
        }

        private void Update<TTime>(IWorld? world, double deltaTime) where TTime : Time
        {
            if (world == null) return;

            if (world.TryGetSingleton(out TTime time))
            {
                time.DeltaTime = (float) deltaTime;
                time.TimeSinceStartup += deltaTime;
            }

            world.Update();
        }
    }
}