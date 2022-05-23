using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Silk.NET.Maths;
using Silk.NET.Windowing;
using YaEcs;
using YaEngine.Bootstrap.Silk;
using YaEngine.Import;
using YaEngine.Input.Silk;
using YaEngine.Render.OpenGL;

namespace YaEngine.Bootstrap
{
    public static class SilkBootstrapExtensions
    {
        public static IServiceCollection AddSilk(this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            return serviceCollection
                .ConfigureFromEnvironment<WindowConfig>(configuration)
                .AddSingleton(provider =>
                {
                    var config = provider.GetService<IOptions<WindowConfig>>();
                    var defaultOptions = WindowOptions.Default;
                    if (config?.Value != null)
                    {
                        defaultOptions.Size = new Vector2D<int>(config.Value.Width, config.Value.Height);
                        defaultOptions.Title = config.Value.Title;
                    }
                    
                    return Window.Create(defaultOptions);
                })
                .AddSilkBindings(configuration)
                .AddSingleton<MeshImporter>()
                .AddSingleton<AnimationImporter>()
                .AddSingleton<AvatarImporter>()
                .AddSingleton<SilkBootstrapper>();
        }

        public static IServiceCollection AddSilkBindings(this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            return serviceCollection
                .AddSingleton<IInitializeSystem, BindSilkInputSystem>()
                .AddSingleton<IInitializeSystem, BindSilkApplicationSystem>()
                .AddSingleton<IInitializeSystem, BindSilkRenderApiSystem>();
        }
    }
}