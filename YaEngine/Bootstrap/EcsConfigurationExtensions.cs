using Microsoft.Extensions.DependencyInjection;
using YaEcs;
using YaEcs.Bootstrap;
using YaEngine.Animation;
using YaEngine.Core;
using YaEngine.ImGui;
using YaEngine.Input;
using YaEngine.Physics;
using YaEngine.Render;
using YaEngine.SceneManagement;
using YaEngine.VFX.ParticleSystem;

namespace YaEngine.Bootstrap
{
    public static class EcsConfigurationExtensions
    {
        public static IServiceCollection AddDefaultSystems(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddDefaultInitializeSystems()
                .AddDefaultUpdateSystems()
                .AddDefaultDisposeSystems()
                .AddModelSteps()
                .AddRenderSteps()
                .AddPhysicsSteps();
        }

        public static IServiceCollection AddScene<T>(this IServiceCollection serviceCollection) where T : class, ISceneProvider
        {
            return serviceCollection
                .AddSingleton<ISceneProvider, T>();
        }
        
        public static IServiceCollection AddDefaultInitializeSystems(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddScoped<IInitializeSystem, InitializeTimeSystem>()
                .AddScoped<IInitializeSystem, InitializeShadersSystem>()
                .AddScoped<IInitializeSystem, InitializeTexturesSystem>()
                .AddScoped<IInitializeSystem, InitializeImGuiSystem>()
                .AddScoped<IInitializeSystem, InitializeCameraRegistrySystem>()
                .AddScoped<IInitializeSystem, InitializeParticlesSystem>()
                .AddScoped<IInitializeSystem, InitializeBuffersSystem>()
                .AddScoped<IInitializeSystem, InitializePhysicsSystem>();
        }
        
        public static IServiceCollection AddDefaultUpdateSystems(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddScoped<IUpdateSystem, InputSystem>()
                .AddScoped<IUpdateSystem, RegisterCameraSystem>()
                .AddScoped<IUpdateSystem, AnimatorUpdateSystem>()
                .AddScoped<IUpdateSystem, ParticleSystem>()
                .AddScoped<IUpdateSystem, RenderSystem>()
                .AddScoped<IUpdateSystem, ImGuiSystem>()
                .AddScoped<IUpdateSystem, UpdatePhysicsSystem>()
                .AddScoped<IUpdateSystem, SetTransformsSystem>();
        }
        
        public static IServiceCollection AddDefaultDisposeSystems(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddScoped<IDisposeSystem, DisposeShadersSystem>()
                .AddScoped<IDisposeSystem, DisposeTextureSystem>()
                .AddScoped<IDisposeSystem, DisposeImGuiSystem>()
                .AddScoped<IDisposeSystem, DisposeEntitiesSystem>();
        }
        
        public static IServiceCollection AddModelSteps(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddScoped<UpdateStep>(_ => ModelSteps.First)
                .AddScoped<UpdateStep>(_ => ModelSteps.EarlyUpdate)
                .AddScoped<UpdateStep>(_ => ModelSteps.Update)
                .AddScoped<UpdateStep>(_ => ModelSteps.LateUpdate);
        }
        
        public static IServiceCollection AddRenderSteps(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddScoped<UpdateStep>(_ => RenderSteps.Render)
                .AddScoped<UpdateStep>(_ => RenderSteps.ImGui);
        }
        
        public static IServiceCollection AddPhysicsSteps(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddScoped<UpdateStep>(_ => PhysicsSteps.CreateColliders)
                .AddScoped<UpdateStep>(_ => PhysicsSteps.Raycast)
                .AddScoped<UpdateStep>(_ => PhysicsSteps.EarlyUpdatePhysics)
                .AddScoped<UpdateStep>(_ => PhysicsSteps.UpdatePhysics)
                .AddScoped<UpdateStep>(_ => PhysicsSteps.AfterPhysicsUpdate)
                .AddScoped<UpdateStep>(_ => PhysicsSteps.LatePhysicsUpdate);
        }
    }
}