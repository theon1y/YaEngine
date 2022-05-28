using System.Numerics;
using YaEcs;
using YaEngine.Animation;
using YaEngine.Bootstrap;
using YaEngine.Core;

namespace YaEngine.Render
{
    public class RenderSystem : IRenderSystem
    {
        public UpdateStep UpdateStep => RenderSteps.Render;

        public void Execute(IWorld world)
        {
            if (!world.TryGetSingleton(out RenderApi renderApi)) return;
            if (!world.TryGetSingleton(out Application application)) return;
            if (!world.TryGetSingleton(out CameraRegistry cameraRegistry)) return;
            if (!world.TryGetSingleton(out RenderBuffers buffers)) return;

            var lightColor = Vector3.Zero;
            var lightPosition = Vector3.Zero;
            world.ForEach((Entity _, AmbientLight light, Transform transform) =>
            {
                lightColor = light.Color;
                lightPosition = transform.GetWorldPosition();
            });

            renderApi.Clear();

            foreach (var cameraEntity in cameraRegistry.Cameras)
            {
                if (!world.TryGetComponent(cameraEntity, out Camera camera)) continue;
                if (!world.TryGetComponent(cameraEntity, out Transform cameraTransform)) continue;
                
                buffers.OpaqueRenderQueue.Clear();
                buffers.TransparentRenderQueue.Clear();
                world.ForEach((Entity _, Renderer renderer, Transform rendererTransform) =>
                {
                    if (!renderer.IsEnabled) return;
                    
                    var renderArguments = new RenderArguments
                    {
                        Renderer = renderer,
                        RendererTransform = rendererTransform
                    };
                    if (renderer.Material.Blending == Blending.Disabled)
                    {
                        buffers.OpaqueRenderQueue.Add(renderArguments);
                    }
                    else
                    {
                        buffers.TransparentRenderQueue.Add(renderArguments);
                    }
                });
                
                var windowSize = application.Instance.Size;
                var aspectRatio = windowSize.X / (float) windowSize.Y;
                var fov = camera.Fov.ToRadians();
                var forward = Vector3.Normalize(Vector3.Transform(Vector3.UnitZ, cameraTransform.Rotation));
                var view = Matrix4x4.CreateLookAt(cameraTransform.Position, cameraTransform.Position + forward,
                    Vector3.UnitY);
                var cameraPosition = cameraTransform.Position;
                var projection = Matrix4x4.CreatePerspectiveFieldOfView(fov, aspectRatio, 0.1f, 100f);

                //buffers.OpaqueRenderQueue.Sort(SortRenderers);                
                renderApi.PrepareOpaqueRender();
                foreach (var renderArguments in buffers.OpaqueRenderQueue)
                {
                    Render(renderArguments.Renderer, renderArguments.RendererTransform, view, projection, renderApi, lightColor,
                        lightPosition, cameraPosition);
                }

                //buffers.TransparentRenderQueue.Sort(SortRenderers);
                renderApi.PrepareTransparentRender();
                foreach (var renderArguments in buffers.TransparentRenderQueue)
                {
                    Render(renderArguments.Renderer, renderArguments.RendererTransform, view, projection, renderApi, lightColor,
                        lightPosition, cameraPosition);
                }
            }
        }

        private static void Render(Renderer renderer, Transform rendererTransform, Matrix4x4 view, Matrix4x4 projection,
            RenderApi renderApi, Vector3 lightColor, Vector3 lightPosition, Vector3 cameraPosition)
        {
            renderer.Update();
            renderer.Bind();

            var shader = renderer.Material.Shader;
            shader.Use();

            var worldMatrix = rendererTransform.GetWorldMatrix();
            shader.SetUniform("uModel", worldMatrix);
            shader.SetUniform("uView", view);
            shader.SetUniform("uProjection", projection);
            shader.TrySetUniform("uViewPosition", cameraPosition);

            shader.TrySetUniform("uLightColor", lightColor);
            shader.TrySetUniform("uLightPosition", lightPosition);
            
            foreach (var uniform in renderer.Material.Vector4Uniforms)
            {
                shader.TrySetUniform(uniform.Key, uniform.Value);
            }
            
            foreach (var uniform in renderer.Material.FloatUniforms)
            {
                shader.TrySetUniform(uniform.Key, uniform.Value);
            }

            var slot = 0;
            foreach (var uniform in renderer.Material.TextureUniforms)
            {
                if (!shader.TryGetUniformLocation(uniform.Key, out var textureLocation)) continue;

                uniform.Value.Bind(slot);
                shader.SetUniform(textureLocation, slot);
                ++slot;
            }

            if (renderer.BoneMatrices != null && shader.TryGetUniformLocation("uFinalBoneMatrices", out var boneMatricesLocation))
            {
                shader.SetUniform(boneMatricesLocation, renderer.BoneMatrices);
            }
            
            renderApi.Draw(renderer);
        }
    }
}