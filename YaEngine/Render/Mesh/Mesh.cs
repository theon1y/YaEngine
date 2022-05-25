using System.Collections.Generic;

namespace YaEngine.Render
{
    public class Mesh
    {
        public float[] Vertices;
        public uint[] Indexes;

        public List<MeshAttribute> Attributes = new();
        public uint VertexSize;
    }
}