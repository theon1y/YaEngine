namespace YaEngine.Render.OpenGL
{
    public class GlRenderer : Renderer
    {
        public BufferObject<uint> Ebo;
        public BufferObject<float> Vbo;
        public BufferObject<float>? InstanceVbo;
        public VertexArrayObject<float> Vao;

        public override void Bind()
        {
            Vao.Bind();
            Ebo.Bind();
        }

        public override void Update()
        {
            InstanceVbo?.Bind();
            InstanceVbo?.Update(InstanceData.Vertices);
        }
    }
}