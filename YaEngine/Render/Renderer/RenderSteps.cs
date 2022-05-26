using YaEcs;

namespace YaEngine.Render
{
    public class RenderSteps
    {
        public static readonly UpdateStep Render = new(nameof(Render), 100);
        public static readonly UpdateStep ImGui = new(nameof(ImGui), 200);
    }
}