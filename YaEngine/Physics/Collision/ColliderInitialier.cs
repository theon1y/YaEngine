using System.Numerics;
using YaEcs;

namespace YaEngine.Physics
{
    public class ColliderInitializer : IComponent
    {
        public ColliderType Type = ColliderType.BoxShape;
        public Vector3 Size = Vector3.One;
        public Vector3 Offset = Vector3.Zero;
        public float Mass;
    }
}