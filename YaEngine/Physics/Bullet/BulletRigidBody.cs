using System.Numerics;

namespace YaEngine.Physics
{
    public class BulletRigidBody : RigidBody
    {
        public BulletSharp.RigidBody Value;

        public override bool IsActive => Value.IsActive;
        public override Matrix4x4 WorldTransform => Value.MotionState.WorldTransform.ToNative();
    }
}