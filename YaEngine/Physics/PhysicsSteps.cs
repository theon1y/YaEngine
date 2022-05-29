using YaEcs;

namespace YaEngine.Physics
{
    public record PhysicsStep(string Name, int Priority) : UpdateStep(Name, Priority), IPhysicsMarker;
    
    public static class PhysicsSteps
    {
        public static readonly PhysicsStep CreateColliders = new(nameof(CreateColliders), 1000);
        public static readonly PhysicsStep Raycast = new(nameof(Raycast), 2000);
        public static readonly PhysicsStep EarlyUpdatePhysics = new(nameof(EarlyUpdatePhysics), 3000);
        public static readonly PhysicsStep UpdatePhysics = new(nameof(UpdatePhysics), 4000);
        public static readonly PhysicsStep AfterPhysicsUpdate = new(nameof(AfterPhysicsUpdate), 5000);
        public static readonly PhysicsStep LatePhysicsUpdate = new(nameof(LatePhysicsUpdate), 6000);
    }
}