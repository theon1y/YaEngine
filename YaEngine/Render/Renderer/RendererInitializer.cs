using System.Numerics;
using YaEcs;

namespace YaEngine.Render
{
    public class RendererInitializer : IComponent
    {
        public Mesh Mesh;
        public MaterialInitializer Material;
        public bool CullFace = true;
        public Mesh? InstanceData;
        public Matrix4x4[]? BoneMatrices;
    }
}