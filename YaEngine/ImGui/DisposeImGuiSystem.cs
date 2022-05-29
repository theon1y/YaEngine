using System.Threading.Tasks;
using YaEcs;
using YaEngine.Bootstrap;
using YaEngine.Model;

namespace YaEngine.ImGui
{
    public class DisposeImGuiSystem : IDisposeModelSystem
    {
        public int Priority => DisposePriorities.First;
        
        public Task ExecuteAsync(IWorld world)
        {
            if (!world.TryGetSingleton(out GuiRegistry guiRegistry)) return Task.CompletedTask;
            
            guiRegistry.Systems.Clear();
            return Task.CompletedTask;
        }
    }
}