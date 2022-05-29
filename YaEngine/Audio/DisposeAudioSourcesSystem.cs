using System.Threading.Tasks;
using YaEcs;
using YaEngine.Bootstrap;
using YaEngine.Model;

namespace YaEngine.Audio
{
    public class DisposeAudioSourcesSystem : IDisposeModelSystem
    {
        public int Priority => DisposePriorities.First;
        
        public Task ExecuteAsync(IWorld world)
        {
            world.ForEach((Entity _, AudioSource audioSource) =>
            {
                audioSource.Dispose();
            });
            return Task.CompletedTask;
        }
    }
}