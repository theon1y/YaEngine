using YaEcs;

namespace YaEngine.Render
{
    public abstract class Renderer : IComponent
    {
        public Mesh Mesh;
        public Material Material;

        public abstract void Bind();
    }
}