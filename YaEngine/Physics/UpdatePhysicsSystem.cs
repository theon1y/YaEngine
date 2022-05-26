using YaEcs;

namespace YaEngine.Physics
{
    public class UpdatePhysicsSystem : IPhysicsSystem
    {
        public UpdateStep UpdateStep => PhysicsSteps.UpdatePhysics;
        
        public void Execute(IWorld world)
        {
            if (!world.TryGetSingleton(out Physics physics)) return;
            if (!world.TryGetSingleton(out PhysicsTime time)) return;

            physics.Update(time.DeltaTime);
        }
    }
}