using Silk.NET.OpenAL;

namespace YaEngine.Audio.OpenAL
{
    public class AlAudioApi : AudioApi
    {
        internal AL Al;
        internal ALContext AlContext;
        internal unsafe Context* ContextPointer;
        internal unsafe Device* DevicePointer;
    }
}