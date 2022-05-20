using System.Threading.Tasks;
using YaEcs;

namespace YaEngine.Input
{
    public class DisposeInputSystem : IDisposeSystem
    {
        public Task ExecuteAsync(IWorld world)
        {
            if (!world.TryGetSingleton(out InputContext inputContext)) return Task.CompletedTask;
            
            inputContext.Instance.Dispose();
            return Task.CompletedTask;
        }
    }
}