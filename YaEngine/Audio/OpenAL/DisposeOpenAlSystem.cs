using System.Threading.Tasks;
using YaEcs;

namespace YaEngine.Audio.OpenAL
{
    public class DisposeOpenAlSystem : IDisposeSystem
    {
        public Task ExecuteAsync(IWorld world)
        {
            if (!world.TryGetSingleton(out AudioApi api) || api is not AlAudioApi alAudioApi) return Task.CompletedTask;
            
            unsafe
            {
                alAudioApi.AlContext.DestroyContext(alAudioApi.ContextPointer);
                alAudioApi.AlContext.CloseDevice(alAudioApi.DevicePointer);
                alAudioApi.Al.Dispose();
                alAudioApi.AlContext.Dispose();
            }
            return Task.CompletedTask;
        }
    }
}