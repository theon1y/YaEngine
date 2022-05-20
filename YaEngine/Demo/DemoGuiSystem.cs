using YaEcs;
using YaEngine.ImGui;

namespace YaEngine
{
    public class DemoGuiSystem : IImGuiSystem
    {
        public void Execute(IWorld world)
        {
            ImGuiNET.ImGui.ShowDemoWindow();
        }
    }
}