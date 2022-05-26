using System.Numerics;
using BulletSharp;

namespace YaEngine.Physics
{
    public class BulletPhysicsCollider : Collider
    {
        public CollisionShape CollisionShape;
        public Vector3 Size;
        public Vector3 Offset;
    }
}