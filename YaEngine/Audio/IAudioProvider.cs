using System;
using System.Threading;
using System.Threading.Tasks;

namespace YaEngine.Audio
{
    public interface IAudioProvider : IDisposable
    {
        AudioProperties ReadProperties();
        byte[] GetAudioData();
        Task<byte[]> GetAudioDataAsync(CancellationToken ct = default);
    }
}