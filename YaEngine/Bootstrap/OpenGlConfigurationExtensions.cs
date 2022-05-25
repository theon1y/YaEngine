using Microsoft.Extensions.DependencyInjection;
using YaEcs;
using YaEngine.Render;
using YaEngine.Render.OpenGL;

namespace YaEngine.Bootstrap
{
    public static class OpenGlConfigurationExtensions
    {
        public static IServiceCollection AddOpenGl(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddScoped<IInitializeRenderSystem, InitializeGlRenderSystem>()
                .AddScoped<IDisposeSystem, DisposeRenderersGlSystem>()
                .AddScoped<IDisposeSystem, DisposeRenderApiGlSystem>();
        }
    }
}