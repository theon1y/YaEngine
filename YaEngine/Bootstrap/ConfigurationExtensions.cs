using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using YaEngine.SceneManagement;

namespace YaEngine.Bootstrap
{
    public static class ConfigurationExtensions
    {
        /// Creates options using appsettings.json and environment
        public static IServiceCollection ConfigureFromEnvironment<T>(this IServiceCollection services,
            IConfiguration configuration)
            where T : class, new()
        {
            return services.Configure<T>(configuration.GetSection(typeof(T).Name));
        }

        public static IServiceCollection AddDefaultLogging(this IServiceCollection services,
            IConfiguration configuration)
        {
            return services.AddLogging(builder => builder
                .ClearProviders()
                .AddConfiguration(configuration.GetSection("Logging"))
                .AddConsole());
        }

        public static IServiceCollection AddManagers(this IServiceCollection services)
        {
            return services
                .AddSingleton<WorldManager>()
                .AddSingleton<SceneManager>()
                .AddSingleton<ISceneManager>(provider => provider.GetService<SceneManager>());
        }
    }
}