using System.Threading.Tasks;
using YaEcs;

namespace YaEngine.Audio
{
    public class DisposeAudioSourcesSystem : IDisposeSystem
    {
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