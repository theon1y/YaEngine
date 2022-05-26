using YaEcs;
using YaEngine.Core;

namespace YaEngine.Physics
{
    public class SetTransformsSystem : IPhysicsSystem
    {
        public UpdateStep UpdateStep => PhysicsSteps.AfterPhysicsUpdate;
        
        public void Execute(IWorld world)
        {
            world.ForEach((Entity _, RigidBody rigidBody, Transform transform) =>
            {
                var newWorldTransform = rigidBody.WorldTransform;
                transform.SetPositionRotation(newWorldTransform);
            });
        }
    }
}