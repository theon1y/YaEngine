using System.Threading.Tasks;
using YaEcs;

namespace YaEngine.Render.OpenGL
{
    public class DisposeRenderersGlSystem : IDisposeRenderSystem
    {
        public Task ExecuteAsync(IWorld world)
        {
            world.ForEach((Entity _, Renderer renderer) =>
            {
                if (renderer is not GlRenderer glRenderer) return;
                
                glRenderer.Vbo.Dispose();
                glRenderer.Ebo.Dispose();
                glRenderer.Vao.Dispose();
            });
            return Task.CompletedTask;
        }
    }
}