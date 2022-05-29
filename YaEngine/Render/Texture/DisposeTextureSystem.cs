using System.Threading.Tasks;
using YaEcs;
using YaEngine.Bootstrap;

namespace YaEngine.Render
{
    public class DisposeTextureSystem : IDisposeRenderSystem
    {
        public int Priority => DisposePriorities.Second;
        
        public Task ExecuteAsync(IWorld world)
        {
            if (!world.TryGetSingleton(out TextureRegistry registry)) return Task.CompletedTask;

            foreach (var texture in registry.Textures)
            {
                texture.Dispose();
            }
            return Task.CompletedTask;
        }
    }
}