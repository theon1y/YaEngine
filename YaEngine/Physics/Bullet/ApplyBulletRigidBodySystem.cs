using System.Numerics;
using YaEcs;

namespace YaEngine.Physics
{
    public class ApplyBulletRigidBodySystem : IPhysicsSystem
    {
        public UpdateStep UpdateStep => PhysicsSteps.EarlyUpdatePhysics;
        
        public void Execute(IWorld world)
        {
            world.ForEach((Entity _, RigidBody rigidBody) =>
            {
                if (rigidBody.Force == Vector3.Zero) return;
                if (rigidBody is not BulletRigidBody { Value: var rb }) return;
                
                rb.ApplyCentralForce(rigidBody.Force.ToBullet());
                rigidBody.Force *= rigidBody.Dampen;
                if (rigidBody.Force.LengthSquared() < 0.1f)
                {
                    rigidBody.Force = Vector3.Zero;
                }
            });
        }
    }
}