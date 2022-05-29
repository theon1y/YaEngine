using System.Threading.Tasks;
using YaEcs;
using YaEngine.Bootstrap;

namespace YaEngine.Render.OpenGL
{
    public class DisposeRenderersGlSystem : IDisposeRenderSystem
    {
        public int Priority => DisposePriorities.First;
        
        public Task ExecuteAsync(IWorld world)
        {
            world.ForEach((Entity _, Renderer renderer) =>
            {
                if (renderer is not GlRenderer glRenderer) return;
                
                glRenderer.Vbo.Dispose();
                glRenderer.Ebo.Dispose();
                glRenderer.Vao.Dispose();
                glRenderer.InstanceVbo?.Dispose();
            });
            return Task.CompletedTask;
        }
    }
}