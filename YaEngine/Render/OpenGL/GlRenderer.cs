namespace YaEngine.Render.OpenGL
{
    public class GlRenderer : Renderer
    {
        public BufferObject<uint> Ebo;
        public BufferObject<float> Vbo;
        public VertexArrayObject<float, uint> Vao;

        public override void Bind()
        {
            Vao.Bind();
            Ebo.Bind();
        }
    }
}