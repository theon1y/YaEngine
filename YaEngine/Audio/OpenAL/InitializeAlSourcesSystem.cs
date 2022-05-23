using System;
using System.Threading.Tasks;
using YaEcs;
using YaEngine.Bootstrap;

namespace YaEngine.Audio.OpenAL
{
    public class InitializeAlSourcesSystem : IInitializeSystem
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
                component.Load();
                world.AddComponent<AudioSource>(entity, component);
            });
            return Task.CompletedTask;
        }
    }
}