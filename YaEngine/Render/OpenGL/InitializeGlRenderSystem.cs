using System;
using System.Threading.Tasks;
using Silk.NET.OpenGL;
using YaEcs;
using YaEngine.Animation;
using YaEngine.Bootstrap;
using YaEngine.Core;

namespace YaEngine.Render.OpenGL
{
    public class InitializeGlRenderSystem : IInitializeRenderSystem
    {
        public int Priority => InitializePriorities.Fourth;
        
        public Task ExecuteAsync(IWorld world)
        {
            if (!world.TryGetSingleton(out RenderApi renderApi)) return Task.CompletedTask;
            if (!world.TryGetSingleton(out ShaderRegistry shaderRegistry)) return Task.CompletedTask;
            if (!world.TryGetSingleton(out TextureRegistry textureRegistry)) return Task.CompletedTask;
            if (renderApi is not GlRenderApi glRenderApi)
            {
                throw new ArgumentException($"Unsupported render api {renderApi}");
            }

            var gl = glRenderApi.Gl;
            var shaderFactory = new GlShaderFactory(gl);
            world.AddSingleton<IShaderFactory>(shaderFactory);

            var textureFactory = new GlTextureFactory(gl);
            world.AddSingleton<ITextureFactory>(textureFactory);

            world.ForEach((Entity entity, RendererInitializer initializeRenderer) =>
            {
                var renderer = InitializeRenderer(shaderFactory, textureFactory, initializeRenderer, shaderRegistry,
                    textureRegistry, gl);
                world.AddComponent(entity, renderer);
            });
            
            return Task.CompletedTask;
        }

        private static Renderer InitializeRenderer(IShaderFactory shaderFactory, ITextureFactory textureFactory,
            RendererInitializer rendererInitializer, ShaderRegistry shaderRegistry, TextureRegistry textureRegistry, GL gl)
        {
            var material = rendererInitializer.Material;
            var mesh = rendererInitializer.Mesh;
            var renderer = new GlRenderer
            {
                Material = new Material
                {
                    Vector4Uniforms = rendererInitializer.Material.Vector4Uniforms.Copy()
                },
                Mesh = mesh
            };
            
            renderer.Material.Shader = LoadShader(shaderFactory, shaderRegistry, material.ShaderInitializer);
            renderer.Material.Texture = LoadTexture(textureFactory, textureRegistry, material.TextureInitializer);

            BindBuffers(gl, renderer, mesh);
            MapAttributes(renderer, mesh);

            return renderer;
        }

        private static void MapAttributes(GlRenderer renderer, Mesh mesh)
        {
            PointIfPresent(renderer, "vPos", 3, mesh.PositionOffset);
            PointIfPresent(renderer, "vColor", 4, mesh.ColorOffset);
            PointIfPresent(renderer, "vUv", 2, mesh.Uv0Offset);
            PointIfPresent(renderer, "vUv1", 2, mesh.Uv1Offset);
            PointIfPresent(renderer, "vNormal", 3, mesh.NormalOffset);
            PointIfPresent(renderer, "vBoneWeights", Bone.MaxNesting, mesh.BoneWeightOffset);
            PointIfPresent(renderer, "vBoneIds", Bone.MaxNesting, mesh.BoneIdOffset);
        }

        private static void BindBuffers(GL gl, GlRenderer renderer, Mesh mesh)
        {
            renderer.Ebo = new BufferObject<uint>(gl, mesh.Indexes, BufferTargetARB.ElementArrayBuffer,
                BufferUsageARB.StaticDraw);
            renderer.Vbo = new BufferObject<float>(gl, mesh.Vertices, BufferTargetARB.ArrayBuffer,
                BufferUsageARB.StaticDraw);
            renderer.Vao = new VertexArrayObject<float, uint>(gl, renderer.Vbo, renderer.Ebo);
        }

        private static ITexture LoadTexture(ITextureFactory factory, TextureRegistry registry, TextureInitializer initializer)
        {
            if (initializer == null) return LoadTexture(factory, registry, BlackWhiteTexture.Value);
            
            if (registry.TryGet<GlTexture>(initializer.Name, out var texture))
            {
                return texture;
            }
            
            var newTexture = factory.Create(initializer.Name, initializer.Provider);
            registry.Add(newTexture);
            return newTexture;
        }

        private static IShader LoadShader(IShaderFactory factory, ShaderRegistry registry, ShaderInitializer initializer)
        {
            if (initializer == null) return LoadShader(factory, registry, NoShader.Value);
            
            if (registry.TryGet<GlShader>(initializer.Name, out var shader))
            {
                return shader;
            }
            
            var newShader = factory.Create(initializer);
            registry.Add(newShader);
            return newShader;
        }

        private static void PointIfPresent(GlRenderer renderer, string attributeName, int attributeSize,
            int attributeOffset, VertexAttribPointerType type = VertexAttribPointerType.Float)
        {
            if (attributeOffset < 0) return;
            if (!renderer.Material.Shader!.TryGetAttributeLocation(attributeName, out var location)) return;
            
            renderer.Vao.VertexAttributePointer((uint)location, attributeSize, type,
                renderer.Mesh.VertexSize, attributeOffset);
        }
    }
}