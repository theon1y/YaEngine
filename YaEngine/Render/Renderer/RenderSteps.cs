using YaEcs;

namespace YaEngine.Render
{
    public class RenderSteps
    {
        public static readonly UpdateStep Render = new(nameof(Render), 8192);
        public static readonly UpdateStep ImGui = new(nameof(ImGui), 16384);
    }
}