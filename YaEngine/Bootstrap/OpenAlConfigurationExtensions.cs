using Microsoft.Extensions.DependencyInjection;
using YaEcs;
using YaEngine.Audio.OpenAL;

namespace YaEngine.Bootstrap
{
    public static class OpenAlConfigurationExtensions
    {
        public static IServiceCollection AddOpenAl(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddScoped<IInitializeSystem, InitializeOpenAlSystem>()
                .AddScoped<IInitializeSystem, InitializeAlSourcesSystem>()
                .AddScoped<IDisposeSystem, DisposeOpenAlSystem>();
        }
    }
}