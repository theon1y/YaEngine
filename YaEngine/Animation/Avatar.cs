using System.Collections.Generic;

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

        public AvatarNode(string name, int parentIndex)
        {
            Name = name;
            ParentIndex = parentIndex;
        }
    }
}