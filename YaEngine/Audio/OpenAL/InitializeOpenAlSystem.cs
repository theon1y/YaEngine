using System;
using System.Threading.Tasks;
using Silk.NET.OpenAL;
using YaEcs;
using YaEngine.Bootstrap;

namespace YaEngine.Audio.OpenAL
{
    public class InitializeOpenAlSystem : IInitializeSystem
    {
        public int Priority => InitializePriorities.First;
        
        public Task ExecuteAsync(IWorld world)
        {
            unsafe
            {
                var alc = ALContext.GetApi();
                var al = AL.GetApi();
                var device = alc.OpenDevice("");
                if (device == null)
                {
                    throw new Exception("Could not create device");
                }

                var context = alc.CreateContext(device, null);
                alc.MakeContextCurrent(context);
            
                var error = al.GetError();
                if (error != AudioError.NoError)
                {
                    throw new Exception($"OpenAl create context error: {error}");
                }
                
                world.AddSingleton<AudioApi>(new AlAudioApi
                {
                    Al = al,
                    AlContext = alc,
                    ContextPointer = context,
                    DevicePointer = device
                });
            }
            
            return Task.CompletedTask;
        }
    }
}