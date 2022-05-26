using System.Collections.Generic;
using System.Numerics;
using YaEcs;

namespace YaEngine.Core
{
    public class Transform : IComponent
    {
        public Vector3 Position = Vector3.Zero;
        public Vector3 Scale = Vector3.One;
        public Quaternion Rotation = Quaternion.Identity;
        public Transform? Parent;
        public List<Transform> Children = new();
    }
}