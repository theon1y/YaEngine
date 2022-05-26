using System.Numerics;
using BulletSharp;

namespace YaEngine.Physics
{
    public class BulletPhysics : Physics
    {
        public readonly DiscreteDynamicsWorld World;
        public readonly CollisionDispatcher Dispatcher;
        public readonly BroadphaseInterface Broadphase;
        public readonly ConstraintSolver? ConstraintSolver;
        public readonly CollisionConfiguration CollisionConfiguration;

        public BulletPhysics(DiscreteDynamicsWorld world, CollisionDispatcher dispatcher,
            BroadphaseInterface broadphase, ConstraintSolver? constraintSolver, CollisionConfiguration collisionConf)
        {
            World = world;
            Dispatcher = dispatcher;
            Broadphase = broadphase;
            ConstraintSolver = constraintSolver;
            CollisionConfiguration = collisionConf;
        }

        public override void Update(float deltaTime)
        {
            World.StepSimulation(deltaTime);
        }

        public override Raycast Raycast(Vector3 from, Vector3 to)
        {
            var rayFromWorld = from.ToBullet();
            var rayToWorld = to.ToBullet();
            using var cb = new ClosestRayResultCallback(ref rayFromWorld, ref rayToWorld);
            World.RayTest(rayFromWorld, rayToWorld, cb);

            var result = new Raycast { From = from, To = to, IsHit = cb.HasHit };
            if (!cb.HasHit) return result;

            result.Hit = cb.HitPointWorld.ToNative();
            result.Normal = Vector3.Normalize(cb.HitNormalWorld.ToNative());
            
            return result;
        }
    }
}