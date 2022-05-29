using System.Threading.Tasks;
using BulletSharp;
using YaEcs;
using YaEngine.Bootstrap;

namespace YaEngine.Physics
{
    public class DisposeBulletPhysicsSystem : IDisposePhysicsSystem
    {
        public int Priority => DisposePriorities.First;
        
        public Task ExecuteAsync(IWorld world)
        {
            if (!world.TryGetSingleton(out Physics physics) || physics is not BulletPhysics bulletPhysics)
            {
                return Task.CompletedTask;
            }

            var physicsWorld = bulletPhysics.World;
            for (var i = physicsWorld.NumConstraints - 1; i >= 0; i--)
            {
                TypedConstraint constraint = physicsWorld.GetConstraint(i);
                physicsWorld.RemoveConstraint(constraint);
                constraint.Dispose();
            }

            //remove the rigidbodies from the dynamics world and delete them
            for (var i = physicsWorld.NumCollisionObjects - 1; i >= 0; i--)
            {
                CollisionObject obj = physicsWorld.CollisionObjectArray[i];
                if (obj is BulletSharp.RigidBody { MotionState: var motionState })
                {
                    motionState?.Dispose();
                }
                physicsWorld.RemoveCollisionObject(obj);
                obj.Dispose();
            }

            world.ForEach((Entity _, Collider collider) =>
            {
                if (collider is not BulletPhysicsCollider bulletPhysicsCollider) return;
                
                bulletPhysicsCollider.CollisionShape.Dispose();
            });
            
            return Task.CompletedTask;
        }
    }
}