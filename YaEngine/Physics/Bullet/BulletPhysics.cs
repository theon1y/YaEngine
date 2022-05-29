using System.Numerics;
using BulletSharp;

namespace YaEngine.Physics
{
    public class BulletPhysics : Physics
    {
        public readonly DiscreteDynamicsWorld World;

        public BulletPhysics(DiscreteDynamicsWorld world)
        {
            World = world;
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