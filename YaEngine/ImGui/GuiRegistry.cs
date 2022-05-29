using System.Collections.Generic;
using YaEcs;

namespace YaEngine.ImGui
{
    public class GuiRegistry : IComponent
    {
        public readonly List<IImGuiSystem> Systems = new();
    }
}