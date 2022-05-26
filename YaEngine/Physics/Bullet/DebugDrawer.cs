using YaEcs;
using YaEngine.Core;
using YaEngine.Render;

namespace YaEngine.Physics
{
    public class DebugDraw : IComponent
    {
        public Entity RenderEntity;
    }

    public class DebugDrawSystem : IPhysicsSystem
    {
        public UpdateStep UpdateStep => PhysicsSteps.UpdatePhysics;
        
        public void Execute(IWorld world)
        {
            world.ForEach((Entity _, DebugDraw draw, RigidBody rigidBody) =>
            {
                if (rigidBody is not BulletRigidBody { Value: var rb }) return;
                if (!world.TryGetComponent(draw.RenderEntity, out Renderer renderer)) return;
                if (!world.TryGetComponent(draw.RenderEntity, out Transform transform)) return;

                renderer.PrimitiveType = Primitive.Line;
                
                rb.GetAabb(out var min, out var max);
                
                var center = (max + min) * 0.5f;
                var scale = max - min;
                
                transform.Position = center.ToNative();
                transform.Scale = scale.ToNative();
            });
        }
    }
}