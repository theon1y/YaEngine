using YaEcs;
using YaEngine.Bootstrap;
using YaEngine.Core;
using YaEngine.Render;

namespace YaEngine.ImGui
{
    public class ImGuiSystem : IRenderSystem
    {
        public UpdateStep UpdateStep => RenderSteps.ImGui;
        
        public void Execute(IWorld world)
        {
            if (!world.TryGetSingleton(out ImGui imGui)) return;
            if (!world.TryGetSingleton(out Time time)) return;
            if (!world.TryGetSingleton(out GuiRegistry registry)) return;
            
            imGui.Controller.Update(time.DeltaTime);
            
            foreach (var guiSystem in registry.Systems)
            {
                guiSystem.Execute(world);
            }
            
            imGui.Controller.Render();
        }
    }
}