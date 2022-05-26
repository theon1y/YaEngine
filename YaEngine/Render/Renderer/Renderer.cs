using YaEcs;

namespace YaEngine.Render
{
    public abstract class Renderer : IComponent
    {
        public bool IsEnabled = true;
        public bool CullFace = true;
        
        public uint InstanceCount;
        public Mesh? InstanceData;
        
        public Mesh Mesh;
        public Material Material;

        public Primitive PrimitiveType = Primitive.Triangle;

        public abstract void Bind();
        public abstract void Update();
    }
}