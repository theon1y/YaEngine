using System.Threading.Tasks;
using Silk.NET.OpenGL;
using YaEcs;
using YaEngine.Bootstrap;
using YaEngine.Render.OpenGL;

namespace YaEngine.Render
{
    public class InitializeRenderersSystem : IInitializeSystem
    {
        public int Priority => InitializePriorities.Fourth;
        
        public Task ExecuteAsync(IWorld world)
        {
            if (!world.TryGetSingleton(out RenderApi renderApi)) return Task.CompletedTask;
            if (!world.TryGetSingleton(out ShaderRegistry shaderRegistry)) return Task.CompletedTask;
            if (!world.TryGetSingleton(out TextureRegistry textureRegistry)) return Task.CompletedTask;

            var gl = renderApi.Value;
            world.ForEach((Entity entity, InitializeRenderer initializeRenderer) =>
            {
                InitializeRenderer(world, initializeRenderer, shaderRegistry, textureRegistry, gl, entity);
            });
            
            return Task.CompletedTask;
        }

        private static void InitializeRenderer(IWorld world, InitializeRenderer initializeRenderer,
            ShaderRegistry shaderRegistry, TextureRegistry textureRegistry, GL gl, Entity entity)
        {
            var material = initializeRenderer.Material.Copy();
            var mesh = initializeRenderer.Mesh;
            var renderer = new Renderer
            {
                Material = material,
                Mesh = mesh
            };
            
            LoadShader(shaderRegistry, gl, material);
            LoadTexture(textureRegistry, gl, material);

            BindBuffers(gl, renderer, mesh);
            MapAttributes(renderer, mesh);

            world.AddComponent(entity, renderer);
        }

        private static void MapAttributes(Renderer? renderer, Mesh? mesh)
        {
            PointIfPresent(renderer, "vPos", 3, mesh.PositionOffset);
            PointIfPresent(renderer, "vColor", 4, mesh.ColorOffset);
            PointIfPresent(renderer, "vUv", 2, mesh.Uv0Offset);
            PointIfPresent(renderer, "vUv1", 2, mesh.Uv1Offset);
            PointIfPresent(renderer, "vNormal", 3, mesh.NormalOffset);
        }

        private static void BindBuffers(GL gl, Renderer renderer, Mesh? mesh)
        {
            renderer.Ebo = new BufferObject<uint>(gl, mesh.Indexes, BufferTargetARB.ElementArrayBuffer,
                BufferUsageARB.StaticDraw);
            renderer.Vbo = new BufferObject<float>(gl, mesh.Vertices, BufferTargetARB.ArrayBuffer,
                BufferUsageARB.StaticDraw);
            renderer.Vao = new VertexArrayObject<float, uint>(gl, renderer.Vbo, renderer.Ebo);
        }

        private static void LoadTexture(TextureRegistry textureRegistry, GL gl, Material material)
        {
            if (textureRegistry.TryGet(material.Texture.Name, out var texture))
            {
                material.Texture = texture;
            }
            else
            {
                texture = material.Texture;
                textureRegistry.Add(texture);
            }

            if (!texture.IsLoaded)
            {
                texture.Load(gl);
            }
        }

        private static void LoadShader(ShaderRegistry shaderRegistry, GL gl, Material material)
        {
            if (shaderRegistry.TryGet(material.Shader.Name, out var shader))
            {
                material.Shader = shader;
            }
            else
            {
                shader = material.Shader;
                shaderRegistry.Add(shader);
            }

            if (!shader.IsLoaded)
            {
                shader.Load(gl);
            }
        }

        private static void PointIfPresent(Renderer renderer, string attributeName, int attributeSize, int attributeOffset)
        {
            if (!renderer.Material.Shader.TryGetAttributeLocation(attributeName, out var location)) return;
            
            if (attributeOffset < 0) return;
            
            renderer.Vao.VertexAttributePointer((uint)location, attributeSize, VertexAttribPointerType.Float,
                renderer.Mesh.VertexSize, attributeOffset);
        }
    }
}