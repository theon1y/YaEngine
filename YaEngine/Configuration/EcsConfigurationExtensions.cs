using Microsoft.Extensions.DependencyInjection;
using YaEcs;
using YaEngine.Core;
using YaEngine.ImGui;
using YaEngine.Input;
using YaEngine.Render;
using YaEngine.Render.OpenGL;

namespace YaEngine.Configuration
{
    public static class EcsConfigurationExtensions
    {
        public static IServiceCollection AddDefaultSystems(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddDefaultInitializeSystems()
                .AddDefaultUpdateSystems()
                .AddDefaultDisposeSystems()
                .AddRenderSteps();
        }
        
        public static IServiceCollection AddDefaultInitializeSystems(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddScoped<IInitializeSystem, InitializeTimeSystem>()
                .AddScoped<IInitializeSystem, InitializeShadersSystem>()
                .AddScoped<IInitializeSystem, InitializeTexturesSystem>()
                .AddScoped<IInitializeSystem, InitializeRenderersSystem>()
                .AddScoped<IInitializeSystem, InitializeImGuiSystem>()
                .AddScoped<IInitializeSystem, InitializeCameraRegistrySystem>();
        }
        
        public static IServiceCollection AddDefaultUpdateSystems(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddScoped<IUpdateSystem, InputSystem>()
                .AddScoped<IUpdateSystem, RegisterCameraSystem>()
                .AddScoped<IRenderSystem, RenderSystem>()
                .AddScoped<IRenderSystem, ImGuiSystem>();
        }
        
        public static IServiceCollection AddDefaultDisposeSystems(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddScoped<IDisposeSystem, DisposeShadersSystem>()
                .AddScoped<IDisposeSystem, DisposeTextureSystem>()
                .AddScoped<IDisposeSystem, DisposeRenderersSystem>()
                .AddScoped<IDisposeSystem, DisposeRenderApiSystem>()
                .AddScoped<IDisposeSystem, DisposeImGuiSystem>()
                .AddScoped<IDisposeSystem, DisposeInputSystem>();
        }
        
        public static IServiceCollection AddRenderSteps(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddScoped(_ => RenderSteps.Render)
                .AddScoped(_ => RenderSteps.ImGui);
        }
    }
}