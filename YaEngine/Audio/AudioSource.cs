using System;
using YaEcs;

namespace YaEngine.Audio
{
    public abstract class AudioSource : IComponent, IDisposable
    {
        public bool IsPlaying { get; protected set; }

        public abstract void Play(bool isLooping);
        public abstract void Stop();
        
        public abstract void Dispose();
    }
}