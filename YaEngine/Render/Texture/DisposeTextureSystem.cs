using System.Threading.Tasks;
using YaEcs;

namespace YaEngine.Render
{
    public class DisposeTextureSystem : IDisposeSystem
    {
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