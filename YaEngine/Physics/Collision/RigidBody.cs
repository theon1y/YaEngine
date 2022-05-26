using System.Numerics;
using YaEcs;

namespace YaEngine.Physics
{
    public abstract class RigidBody : IComponent
    {
        public float Mass;
        public Vector3 Force;
        public float Dampen;
        
        public abstract bool IsActive { get; }
        public abstract Matrix4x4 WorldTransform { get; }
    }
}