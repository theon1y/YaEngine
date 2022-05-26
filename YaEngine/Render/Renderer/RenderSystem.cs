﻿using System.Numerics;
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
            var lightDirection = Vector3.One;
            world.ForEach((Entity _, AmbientLight spotLight, Transform transform) =>
            {
                lightColor = spotLight.Color;
                lightDirection = transform.Position;
            });

            renderApi.Clear();

            foreach (var cameraEntity in cameraRegistry.Cameras)
            {
                if (!world.TryGetComponent(cameraEntity, out Camera camera)) continue;
                if (!world.TryGetComponent(cameraEntity, out Transform cameraTransform)) continue;

                var windowSize = application.Instance.Size;
                var aspectRatio = windowSize.X / (float) windowSize.Y;
                var fov = camera.Fov.ToRadians();
                var forward = Vector3.Normalize(Vector3.Transform(Vector3.UnitZ, cameraTransform.Rotation));
                var view = Matrix4x4.CreateLookAt(cameraTransform.Position, cameraTransform.Position + forward,
                    Vector3.UnitY);
                var projection = Matrix4x4.CreatePerspectiveFieldOfView(fov, aspectRatio, 0.1f, 100f);
                
                buffers.OpaqueRenderQueue.Clear();
                buffers.TransparentRenderQueue.Clear();
                world.ForEach((Entity entity, Renderer renderer, Transform rendererTransform) =>
                {
                    if (!renderer.IsEnabled) return;
                    
                    world.TryGetComponent(entity, out Animator animator);
                    var renderArguments = new RenderArguments
                    {
                        Renderer = renderer,
                        RendererTransform = rendererTransform,
                        Animator = animator
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

                //buffers.OpaqueRenderQueue.Sort(SortRenderers);                
                renderApi.PrepareOpaqueRender();
                foreach (var renderArguments in buffers.OpaqueRenderQueue)
                {
                    Render(renderArguments.Renderer, renderArguments.RendererTransform, view, projection, renderApi, lightColor,
                        lightDirection, renderArguments.Animator?.BoneMatrices);
                }

                //buffers.TransparentRenderQueue.Sort(SortRenderers);
                renderApi.PrepareTransparentRender();
                foreach (var renderArguments in buffers.TransparentRenderQueue)
                {
                    Render(renderArguments.Renderer, renderArguments.RendererTransform, view, projection, renderApi, lightColor,
                        lightDirection, renderArguments.Animator?.BoneMatrices);
                }
            }
        }

        private static void Render(Renderer renderer, Transform rendererTransform, Matrix4x4 view, Matrix4x4 projection,
            RenderApi renderApi, Vector3 lightColor, Vector3 lightDirection, Matrix4x4[]? boneMatrices)
        {
            renderer.Update();
            renderer.Bind();

            var shader = renderer.Material.Shader;
            shader.Use();

            var worldMatrix = rendererTransform.GetWorldMatrix();
            shader.SetUniform("uModel", worldMatrix);
            shader.SetUniform("uView", view);
            shader.SetUniform("uProjection", projection);

            shader.TrySetUniform("lightColor", lightColor);
            shader.TrySetUniform("lightDirection", lightDirection);
            
            foreach (var uniform in renderer.Material.Vector4Uniforms)
            {
                shader.SetUniform(uniform.Key, uniform.Value);
            }

            if (renderer.Material.Texture != null && shader.TryGetUniformLocation("uTexture0", out var textureLocation))
            {
                var slot = 0;
                renderer.Material.Texture.Bind(slot);
                shader.SetUniform(textureLocation, slot);
            }

            if (boneMatrices != null && shader.TryGetUniformLocation("uFinalBoneMatrices", out var boneMatricesLocation))
            {
                shader.SetUniform(boneMatricesLocation, boneMatrices);
            }
            
            renderApi.Draw(renderer);
        }
    }
}