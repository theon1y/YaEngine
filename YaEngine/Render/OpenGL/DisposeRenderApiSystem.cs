using System.Threading.Tasks;
using YaEcs;

namespace YaEngine.Render.OpenGL
{
    public class DisposeRenderApiSystem : IDisposeSystem
    {
        public Task ExecuteAsync(IWorld world)
        {
            if (world.TryGetSingleton(out RenderApi api)) return Task.CompletedTask;
            
            api.Value.Dispose();
            return Task.CompletedTask;
        }
    }
}