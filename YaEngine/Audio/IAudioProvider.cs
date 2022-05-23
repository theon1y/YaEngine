using System;

namespace YaEngine.Audio
{
    public interface IAudioProvider : IDisposable
    {
        AudioProperties ReadProperties();
        byte[] GetAudioData();
    }
}