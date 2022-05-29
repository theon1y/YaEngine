using YaEngine.Model;

namespace YaEcs.Bootstrap
{
    public record ModelStep(string Name, int Priority) : UpdateStep(Name, Priority), IModelMarker;
    
    public static class ModelSteps
    {
        public static ModelStep First = new(nameof(First), 10);
        public static ModelStep EarlyUpdate = new(nameof(EarlyUpdate), 20);
        public static ModelStep Update = new(nameof(Update), 30);
        public static ModelStep LateUpdate = new(nameof(LateUpdate), 40);
    }
}