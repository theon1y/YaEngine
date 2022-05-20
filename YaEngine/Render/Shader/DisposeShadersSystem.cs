using System.Threading.Tasks;
using YaEcs;

namespace YaEngine.Render
{
    public class DisposeShadersSystem : IDisposeSystem
    {
        public Task ExecuteAsync(IWorld world)
        {
            if (!world.TryGetSingleton(out ShaderRegistry registry)) return Task.CompletedTask;

            foreach (var shader in registry.Shaders)
            {
                shader.Dispose();
            }
            
            return Task.CompletedTask;
        }
    }
}