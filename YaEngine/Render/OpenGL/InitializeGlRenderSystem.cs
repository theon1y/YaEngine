using System;
using System.Threading.Tasks;
using Silk.NET.OpenGL;
using YaEcs;
using YaEngine.Bootstrap;
using YaEngine.Core;

namespace YaEngine.Render.OpenGL
{
    public class InitializeGlRenderSystem : IInitializeRenderSystem
    {
        public int Priority => InitializePriorities.Fourth;
        
        public Task ExecuteAsync(IWorld world)
        {
            if (!world.TryGetSingleton(out RenderApi renderApi) || renderApi is not GlRenderApi glRenderApi) return Task.CompletedTask;
            if (!world.TryGetSingleton(out ShaderRegistry shaderRegistry)) return Task.CompletedTask;
            if (!world.TryGetSingleton(out TextureRegistry textureRegistry)) return Task.CompletedTask;
            if (!world.TryGetSingleton(out IShaderFactory shaderFactory)) return Task.CompletedTask;
            if (!world.TryGetSingleton(out ITextureFactory textureFactory)) return Task.CompletedTask;

            world.ForEach((Entity entity, RendererInitializer initializeRenderer) =>
            {
                var renderer = InitializeRenderer(shaderFactory, textureFactory, initializeRenderer, shaderRegistry,
                    textureRegistry, glRenderApi.Gl);
                world.AddComponent(entity, renderer);
            });
            
            return Task.CompletedTask;
        }

        public static Renderer InitializeRenderer(IShaderFactory shaderFactory, ITextureFactory textureFactory,
            RendererInitializer rendererInitializer, ShaderRegistry shaderRegistry, TextureRegistry textureRegistry, GL gl)
        {
            var material = rendererInitializer.Material;
            var mesh = rendererInitializer.Mesh;
            var renderer = new GlRenderer
            {
                Material = new Material
                {
                    Blending = rendererInitializer.Material.Blending,
                    Vector4Uniforms = rendererInitializer.Material.Vector4Uniforms.Copy()
                },
                Mesh = mesh,
                CullFace = rendererInitializer.CullFace,
                InstanceData = rendererInitializer.InstanceData
            };
            
            renderer.Material.Shader = LoadShader(shaderFactory, shaderRegistry, material.ShaderInitializer);
            renderer.Material.Texture = LoadTexture(textureFactory, textureRegistry, material.TextureInitializer);

            BindBuffers(gl, renderer);
            return renderer;
        }

        public static void BindBuffers(GL gl, GlRenderer renderer)
        {
            renderer.Vao = new VertexArrayObject<float>(gl);
            renderer.Ebo = new BufferObject<uint>(gl, renderer.Mesh.Indexes, BufferTargetARB.ElementArrayBuffer,
                BufferUsageARB.StaticDraw);
            renderer.Vbo = new BufferObject<float>(gl, renderer.Mesh.Vertices, BufferTargetARB.ArrayBuffer,
                BufferUsageARB.StaticDraw);
            BindAttributes(renderer.Material.Shader, renderer.Mesh, renderer.Vao, renderer.Vbo, 0);
            if (renderer.InstanceData == null) return;
            
            renderer.InstanceVbo = new BufferObject<float>(gl, renderer.InstanceData.Vertices, BufferTargetARB.ArrayBuffer,
                BufferUsageARB.StreamDraw);   
            BindAttributes(renderer.Material.Shader, renderer.InstanceData, renderer.Vao, renderer.InstanceVbo, 1);
        }

        public static void BindAttributes(IShader? shader, Mesh mesh, VertexArrayObject<float> vao,
            BufferObject<float> vbo, uint divisor)
        {
            if (shader == null) throw new Exception("Shader is null");

            vbo.Bind();
            foreach (var attribute in mesh.Attributes)
            {
                PointIfPresent(vao, shader, attribute, mesh.VertexSize, divisor);
            }
        }

        public static ITexture LoadTexture(ITextureFactory factory, TextureRegistry registry, TextureInitializer initializer)
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

        public static IShader LoadShader(IShaderFactory factory, ShaderRegistry registry, ShaderInitializer initializer)
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

        public static void PointIfPresent<T>(VertexArrayObject<T> vao, IShader shader, MeshAttribute attribute,
            uint vertexSize, uint divisor, VertexAttribPointerType type = VertexAttribPointerType.Float)
            where T : unmanaged
        {
            if (attribute.Offset < 0) return;
            if (!shader.TryGetAttributeLocation(attribute.Name, out var location)) return;

            vao.VertexAttributePointer((uint) location, attribute.Size, type, vertexSize, attribute.Offset, divisor);
        }
    }
}