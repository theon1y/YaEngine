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

        private static void BindBuffers(GL gl, GlRenderer renderer)
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

        private static void BindAttributes(IShader? shader, Mesh mesh, VertexArrayObject<float> vao,
            BufferObject<float> vbo, uint divisor)
        {
            if (shader == null) throw new Exception("Shader is null");

            vbo.Bind();
            foreach (var attribute in mesh.Attributes)
            {
                PointIfPresent(vao, shader, attribute, mesh.VertexSize, divisor);
            }
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

        private static void PointIfPresent<T>(VertexArrayObject<T> vao, IShader shader, MeshAttribute attribute,
            uint vertexSize, uint divisor, VertexAttribPointerType type = VertexAttribPointerType.Float)
            where T : unmanaged
        {
            if (attribute.Offset < 0) return;
            if (!shader.TryGetAttributeLocation(attribute.Name, out var location)) return;

            vao.VertexAttributePointer((uint) location, attribute.Size, type, vertexSize, attribute.Offset, divisor);
        }
    }
}