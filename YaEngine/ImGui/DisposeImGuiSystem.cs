using System.Threading.Tasks;
using YaEcs;

namespace YaEngine.ImGui
{
    public class DisposeImGuiSystem : IDisposeSystem
    {
        public Task ExecuteAsync(IWorld world)
        {
            if (!world.TryGetSingleton(out ImGui imGui)) return Task.CompletedTask;
            
            imGui.Controller.Dispose();
            return Task.CompletedTask;
        }
    }
}