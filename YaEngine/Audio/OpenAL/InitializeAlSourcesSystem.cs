using System;
using System.Threading.Tasks;
using YaEcs;
using YaEngine.Bootstrap;
using YaEngine.Model;

namespace YaEngine.Audio.OpenAL
{
    public class InitializeAlSourcesSystem : IInitializeModelSystem
    {
        public int Priority => InitializePriorities.Fourth;
        
        public Task ExecuteAsync(IWorld world)
        {
            if (!world.TryGetSingleton(out AudioApi api) || api is not AlAudioApi alAudioApi)
            {
                throw new Exception($"Unsupported audio api {api}");
            }
            
            world.ForEach((Entity entity, AudioInitializer audioInitializer) =>
            {
                var component = new OpenAlAudioSource(alAudioApi.Al, audioInitializer.AudioProvider);
                world.AddComponent<AudioSource>(entity, component);
            });
            return Task.CompletedTask;
        }
    }
}