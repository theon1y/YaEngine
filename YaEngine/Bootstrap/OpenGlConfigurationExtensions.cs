using Microsoft.Extensions.DependencyInjection;
using YaEcs;
using YaEngine.Render.OpenGL;

namespace YaEngine.Bootstrap
{
    public static class OpenGlConfigurationExtensions
    {
        public static IServiceCollection AddOpenGl(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddScoped<IInitializeSystem, InitializeGlRenderSystem>()
                .AddScoped<IDisposeSystem, DisposeRenderersGlSystem>()
                .AddScoped<IDisposeSystem, DisposeRenderApiGlSystem>();
        }
    }
}