using System.Threading.Tasks;
using YaEcs;
using YaEngine.Render.OpenGL;

namespace YaEngine.Render
{
    public class DisposeRenderersSystem : IDisposeSystem
    {
        public Task ExecuteAsync(IWorld world)
        {
            if (world.TryGetSingleton(out RenderApi api))
            {
                api.Value.Dispose();
            }
            
            world.ForEach((Entity _, Renderer renderer) =>
            {
                renderer.Vbo.Dispose();
                renderer.Ebo.Dispose();
                renderer.Vao.Dispose();
            });
            return Task.CompletedTask;
        }
    }
}