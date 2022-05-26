using System;
using System.Numerics;
using BulletSharp;
using YaEcs;
using YaEngine.Core;
using Vector3 = System.Numerics.Vector3;

namespace YaEngine.Physics
{
    public class CreateBulletShapesSystem : IPhysicsSystem
    {
        public UpdateStep UpdateStep => PhysicsSteps.CreateColliders;
        
        public void Execute(IWorld world)
        {
            if (!world.TryGetSingleton(out Physics physics) || physics is not BulletPhysics bulletPhysics) return;

            world.ForEach((Entity entity, Transform transform, ColliderInitializer initializer) =>
            {
               CreateRigidBody(world, entity, transform, initializer, bulletPhysics);
            });
        }

        public static void CreateRigidBody(IWorld world, Entity entity, Transform transform,
            ColliderInitializer initializer, BulletPhysics bulletPhysics)
        {
            var worldMatrix = transform.GetWorldMatrix();
            var shape = CreateShape(initializer.Type, initializer.Size, worldMatrix);
            var collider = new BulletPhysicsCollider
            {
                CollisionShape = shape,
                Size = initializer.Size,
                Offset = initializer.Offset
            };
            world.AddComponent<Collider>(entity, collider);
            world.RemoveComponent<ColliderInitializer>(entity);

            var offsetMatrix = Matrix4x4.CreateTranslation(initializer.Offset);
            var transformMatrix = worldMatrix * offsetMatrix;
            var rigidBody = CreateRigidBody(initializer.Mass, transformMatrix, shape);
            bulletPhysics.World.AddRigidBody(rigidBody);
            world.AddComponent<RigidBody>(entity, new BulletRigidBody
            {
                Value = rigidBody,
                Mass = initializer.Mass
            });
        }

        private static CollisionShape CreateShape(ColliderType type, Vector3 size, Matrix4x4 modelMatrix)
        {
            var scaledSize = size * modelMatrix.GetScale() * 0.5f;
            return type switch
            {
                ColliderType.BoxShape => new BoxShape(scaledSize.ToBullet()),
                ColliderType.SphereShape => new SphereShape(scaledSize.X),
                ColliderType.CapsuleShape => new CapsuleShape(scaledSize.X, scaledSize.Y),
                ColliderType.ConeShape => new ConeShape(scaledSize.X, scaledSize.Y),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        private static BulletSharp.RigidBody CreateRigidBody(float mass, Matrix4x4 transform, CollisionShape shape)
        {
            var localInertia = BulletSharp.Math.Vector3.Zero;
            if (mass != 0.0f) shape.CalculateLocalInertia(mass, out localInertia);

            transform.M11 = 1;
            transform.M22 = 1;
            transform.M33 = 1;
            var motionState = new DefaultMotionState(transform.ToBullet());
            var rbInfo = new RigidBodyConstructionInfo(mass, motionState, shape, localInertia);
            var body = new BulletSharp.RigidBody(rbInfo);
            rbInfo.Dispose();

            return body;
        }
    }
}