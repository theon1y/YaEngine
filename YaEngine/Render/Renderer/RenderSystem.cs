using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using YaEcs;
using YaEngine.Animation;
using YaEngine.Bootstrap;
using YaEngine.Core;
using YaEngine.Render.OpenGL;

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

            var ambientColor = Vector4.One;
            if (world.TryGetSingleton(out AmbientLight ambientLight))
            {
                ambientColor = ambientLight.Color;
            }

            var spotlightColor = Vector4.Zero;
            var spotlightPosition = Vector3.One;
            world.ForEach((Entity _, SpotLight spotLight, Transform transform) =>
            {
                spotlightColor = spotLight.Color;
                spotlightPosition = transform.Position;
            });

            var gl = renderApi.Value;
            gl.Enable(EnableCap.DepthTest);
            gl.Disable(EnableCap.CullFace);
            gl.ClearColor(ambientColor.X, ambientColor.Y, ambientColor.Z, ambientColor.W);
            gl.Clear((uint) (ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit));

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
                world.ForEach((Entity _, Renderer renderer, Transform rendererTransform) =>
                {
                    world.TryGetComponent(_, out Animator animator);
                    Render(renderer, rendererTransform, view, projection, gl, ambientColor, spotlightColor,
                        spotlightPosition, animator?.BoneMatrices);
                });
            }
        }

        private void Render(Renderer renderer, Transform rendererTransform, Matrix4x4 view, Matrix4x4 projection,
            GL gl, Vector4 ambientColor, Vector4 spotlightColor, Vector3 spotlightPosition,
            Matrix4x4[]? boneMatrices)
        {
            renderer.Vao.Bind();
            renderer.Ebo.Bind();

            var shader = renderer.Material.Shader;
            shader.Use();

            var model = rendererTransform.ModelMatrix;
            
            shader.SetUniform(gl, "uModel", model);
            shader.SetUniform(gl, "uView", view);
            shader.SetUniform(gl, "uProjection", projection);

            shader.TrySetUniform(gl, "lightColor", new Vector3(spotlightColor.X, spotlightColor.Y, spotlightColor.Z));
            shader.TrySetUniform(gl, "lightPos", spotlightPosition);
            
            foreach (var uniform in renderer.Material.Vector4Uniforms)
            {
                shader.SetUniform(gl, uniform.Key, uniform.Value);
            }

            if (shader.TryGetUniformLocation("uTexture0", out var textureLocation))
            {
                renderer.Material.Texture.Bind();
                gl.Uniform1(textureLocation, 0);
            }

            if (boneMatrices != null && shader.TryGetUniformLocation("uFinalBoneMatrices", out var boneMatricesLocation))
            {
                var span = MemoryMarshal.Cast<Matrix4x4, float>(boneMatrices.AsSpan());
                gl.UniformMatrix4(boneMatricesLocation, (uint) boneMatrices.Length, false, span);
            }

            gl.DrawElements(PrimitiveType.Triangles, (uint) renderer.Mesh.Indexes.Length, DrawElementsType.UnsignedInt, Unsafe.NullRef<uint>());
        }
    }
}