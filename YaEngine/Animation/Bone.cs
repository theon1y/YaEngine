using System.Numerics;

namespace YaEngine.Animation
{
    public class Bone
    {
        public const int MaxNesting = 4;
        
        public readonly int Id;
        public readonly Matrix4x4 BoneSpaceMatrix;
        
        public Bone(int id, Matrix4x4 boneSpaceMatrix)
        {
            Id = id;
            BoneSpaceMatrix = boneSpaceMatrix;
        }
    }
}