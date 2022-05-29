using System.Collections.Generic;
using System.Threading.Tasks;
using YaEcs;
using YaEngine.Bootstrap;
using YaEngine.Render;

namespace YaEngine.ImGui
{
    public class RegisterGuiSystem : IInitializeRenderSystem
    {
        public int Priority => InitializePriorities.Third;

        private readonly IEnumerable<IImGuiSystem> imGuiSystems;
        
        public RegisterGuiSystem(IEnumerable<IImGuiSystem> imGuiSystems)
        {
            this.imGuiSystems = imGuiSystems;
        }
        
        public Task ExecuteAsync(IWorld world)
        {
            if (!world.TryGetSingleton(out GuiRegistry registry)) return Task.CompletedTask;
            
            registry.Systems.AddRange(imGuiSystems);
            return Task.CompletedTask;
        }
    }
}