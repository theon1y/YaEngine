using System.Collections.Generic;
using YaEngine.Animation;

namespace YaEngine.Render
{
    public class Mesh
    {
        public float[] Vertices;
        public uint[] Indexes;
        public Dictionary<string, Bone> Bones;
        
        public uint VertexSize;
        public int PositionOffset = -1;
        public int ColorOffset = -1;
        public int NormalOffset = -1;
        public int Uv0Offset = -1;
        public int Uv1Offset = -1;
        public int BoneIdOffset = -1;
        public int BoneWeightOffset = -1;
    }
}