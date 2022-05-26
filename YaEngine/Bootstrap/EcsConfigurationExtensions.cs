﻿using Microsoft.Extensions.DependencyInjection;
using YaEcs;
using YaEngine.Animation;
using YaEngine.Core;
using YaEngine.ImGui;
using YaEngine.Input;
using YaEngine.Physics;
using YaEngine.Render;
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
                .AddRenderSteps()
                .AddPhysicsSteps();
        }
        
        public static IServiceCollection AddDefaultInitializeSystems(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddScoped<IInitializeSystem, InitializeTimeSystem>()
                .AddScoped<IInitializeSystem, InitializeShadersSystem>()
                .AddScoped<IInitializeSystem, InitializeTexturesSystem>()
                .AddScoped<IInitializeSystem, InitializeImGuiSystem>()
                .AddScoped<IInitializeSystem, InitializeCameraRegistrySystem>()
                .AddScoped<IInitializeRenderSystem, InitializeParticlesSystem>()
                .AddScoped<IInitializeRenderSystem, InitializeBuffersSystem>()
                .AddScoped<IInitializePhysicsSystem, InitializePhysicsSystem>();
        }
        
        public static IServiceCollection AddDefaultUpdateSystems(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddScoped<IUpdateSystem, InputSystem>()
                .AddScoped<IUpdateSystem, RegisterCameraSystem>()
                .AddScoped<IUpdateSystem, AnimatorUpdateSystem>()
                .AddScoped<IUpdateSystem, ParticleSystem>()
                .AddScoped<IRenderSystem, RenderSystem>()
                .AddScoped<IRenderSystem, ImGuiSystem>()
                .AddScoped<IPhysicsSystem, UpdatePhysicsSystem>()
                .AddScoped<IPhysicsSystem, SetTransformsSystem>();
        }
        
        public static IServiceCollection AddDefaultDisposeSystems(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddScoped<IDisposeSystem, DisposeShadersSystem>()
                .AddScoped<IDisposeSystem, DisposeTextureSystem>()
                .AddScoped<IDisposeSystem, DisposeImGuiSystem>()
                .AddScoped<IDisposeSystem, DisposeInputSystem>();
        }
        
        public static IServiceCollection AddRenderSteps(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddScoped(_ => RenderSteps.Render)
                .AddScoped(_ => RenderSteps.ImGui);
        }
        
        public static IServiceCollection AddPhysicsSteps(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddScoped(_ => PhysicsSteps.CreateColliders)
                .AddScoped(_ => PhysicsSteps.Raycast)
                .AddScoped(_ => PhysicsSteps.EarlyUpdatePhysics)
                .AddScoped(_ => PhysicsSteps.UpdatePhysics)
                .AddScoped(_ => PhysicsSteps.AfterPhysicsUpdate)
                .AddScoped(_ => PhysicsSteps.LatePhysicsUpdate);
        }
    }
}