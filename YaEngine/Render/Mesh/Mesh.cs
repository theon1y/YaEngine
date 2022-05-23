namespace YaEngine.Render
{
    public class Mesh
    {
        public float[] Vertices;
        public uint[] Indexes;
        
        public uint VertexSize;
        public int PositionOffset;
        public int ColorOffset = -1;
        public int NormalOffset = -1;
        public int Uv0Offset = -1;
        public int Uv1Offset = -1;
        public int BoneIdOffset = -1;
        public int BoneWeightOffset = -1;
    }
}