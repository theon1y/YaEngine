using System.Numerics;

namespace YaEngine.Animation
{
    public class Bone
    {
        public const int MaxNesting = 4;
        
        public readonly int Id;
        public readonly Matrix4x4 BoneOffset;
        
        public Bone(int id, Matrix4x4 boneOffset)
        {
            Id = id;
            BoneOffset = boneOffset;
        }
    }
}