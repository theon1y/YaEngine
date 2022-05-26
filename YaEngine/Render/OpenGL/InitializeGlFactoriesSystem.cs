using System;
using System.Threading.Tasks;
using YaEcs;
using YaEngine.Bootstrap;

namespace YaEngine.Render.OpenGL
{
    public class InitializeGlFactoriesSystem : IInitializeRenderSystem
    {
        public int Priority => InitializePriorities.Second;

        public Task ExecuteAsync(IWorld world)
        {
            if (!world.TryGetSingleton(out RenderApi renderApi)) return Task.CompletedTask;
            if (renderApi is not GlRenderApi glRenderApi)
            {
                throw new ArgumentException($"Unsupported render api {renderApi}");
            }

            var gl = glRenderApi.Gl;
            var shaderFactory = new GlShaderFactory(gl);
            world.AddSingleton<IShaderFactory>(shaderFactory);

            var textureFactory = new GlTextureFactory(gl);
            world.AddSingleton<ITextureFactory>(textureFactory);

            return Task.CompletedTask;
        }
    }
}