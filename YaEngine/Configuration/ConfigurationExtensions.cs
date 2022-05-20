using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace YaEngine.Configuration
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
    }
}