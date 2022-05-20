using YaEcs;
using YaEngine.Render.OpenGL;

namespace YaEngine.Render
{
    public class Renderer : IComponent
    {
        public Mesh Mesh;
        public Material Material;

        public BufferObject<uint> Ebo;
        public BufferObject<float> Vbo;
        public VertexArrayObject<float, uint> Vao;
    }
}