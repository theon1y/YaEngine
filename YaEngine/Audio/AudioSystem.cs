using YaEcs;
using YaEcs.Bootstrap;

namespace YaEngine.Audio
{
    public class AudioSystem : IUpdateSystem
    {
        public UpdateStep UpdateStep => UpdateSteps.EarlyUpdate;
        
        public void Execute(IWorld world)
        {
            
        }
    }
}