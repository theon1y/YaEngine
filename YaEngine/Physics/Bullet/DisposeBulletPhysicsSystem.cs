using System.Collections.Generic;
using System.Threading.Tasks;
using BulletSharp;
using YaEcs;

namespace YaEngine.Physics
{
    public class DisposeBulletPhysicsSystem : IDisposePhysicsSystem
    {
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

            var collidersToRemove = new List<Entity>();
            world.ForEach((Entity entity, Collider collider) =>
            {
                if (collider is not BulletPhysicsCollider bulletPhysicsCollider) return;
                
                bulletPhysicsCollider.CollisionShape.Dispose();
                collidersToRemove.Add(entity);
            });

            foreach (var entity in collidersToRemove)
            {
                world.RemoveComponent<Collider>(entity);
            }

            bulletPhysics.World.Dispose();
            bulletPhysics.Broadphase.Dispose();
            bulletPhysics.Dispatcher?.Dispose();
            bulletPhysics.ConstraintSolver?.Dispose();
            bulletPhysics.CollisionConfiguration.Dispose();
            
            return Task.CompletedTask;
        }
    }
}