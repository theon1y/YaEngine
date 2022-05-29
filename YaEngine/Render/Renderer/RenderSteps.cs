using YaEcs;

namespace YaEngine.Render
{
    public record RenderStep(string Name, int Priority) : UpdateStep(Name, Priority), IRenderMarker;
    
    public class RenderSteps
    {
        public static readonly RenderStep Render = new(nameof(Render), 100);
        public static readonly RenderStep ImGui = new(nameof(ImGui), 200);
    }
}