using System.Collections.Generic;
using System.Numerics;

namespace YaEngine.Animation
{
    public class Avatar
    {
        public AvatarNode[] Hierarchy;
        public Dictionary<string, Bone> Bones;
    }

    public readonly struct AvatarNode
    {
        public readonly string Name;
        public readonly int ParentIndex;
        public readonly uint[] MeshIndexes;
        public readonly Matrix4x4 LocalTransform;

        public AvatarNode(string name, int parentIndex, uint[] meshIndexes, Matrix4x4 localTransform)
        {
            Name = name;
            ParentIndex = parentIndex;
            MeshIndexes = meshIndexes;
            LocalTransform = localTransform;
        }
    }
}