using System.Threading.Tasks;
using YaEcs;
using YaEngine.Bootstrap;

namespace YaEngine.Render
{
    public class DisposeShadersSystem : IDisposeRenderSystem
    {
        public int Priority => DisposePriorities.Third;
        
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