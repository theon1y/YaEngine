using YaEcs;

namespace YaEngine.Physics
{
    public static class PhysicsSteps
    {
        public static readonly UpdateStep CreateColliders = new(nameof(CreateColliders), 1000);
        public static readonly UpdateStep Raycast = new(nameof(Raycast), 2000);
        public static readonly UpdateStep EarlyUpdatePhysics = new(nameof(EarlyUpdatePhysics), 3000);
        public static readonly UpdateStep UpdatePhysics = new(nameof(UpdatePhysics), 4000);
        public static readonly UpdateStep AfterPhysicsUpdate = new(nameof(AfterPhysicsUpdate), 5000);
        public static readonly UpdateStep LatePhysicsUpdate = new(nameof(LatePhysicsUpdate), 6000);
    }
}