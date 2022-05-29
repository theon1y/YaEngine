using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MP3Sharp;

namespace YaEngine.Audio
{
    public class Mp3AudioProvider : IAudioProvider
    {
        private readonly string filePath;

        public Mp3AudioProvider(string filePath)
        {
            this.filePath = filePath;
        }

        public AudioProperties ReadProperties()
        {
            using var stream = new MP3Stream(filePath);
            return new()
            {
                NumChannels = stream.ChannelCount,
                SampleRate = stream.Frequency,
                BitsPerSample = 16,
            };
        }

        public byte[] GetAudioData()
        {
            using var stream = new MP3Stream(filePath);
            using var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }

        public async Task<byte[]> GetAudioDataAsync(CancellationToken ct = default)
        {
            using var stream = new MP3Stream(filePath);
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream, ct);
            return memoryStream.ToArray();
        }
        
        public void Dispose()
        {
            
        }
    }
}