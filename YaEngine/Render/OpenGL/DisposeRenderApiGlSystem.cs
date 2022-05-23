using System.Threading.Tasks;
using YaEcs;

namespace YaEngine.Render.OpenGL
{
    public class DisposeRenderApiGlSystem : IDisposeRenderSystem
    {
        public Task ExecuteAsync(IWorld world)
        {
            if (world.TryGetSingleton(out RenderApi api) && api is GlRenderApi glRenderApi)
            {
                glRenderApi.Gl.Dispose();
            }
            
            return Task.CompletedTask;
        }
    }
}