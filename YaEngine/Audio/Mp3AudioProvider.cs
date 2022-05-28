using System;
using System.Collections.Generic;
using System.IO;
using MP3Sharp;

namespace YaEngine.Audio
{
    public class Mp3AudioProvider : IAudioProvider
    {
        private readonly MP3Stream stream;

        public Mp3AudioProvider(string filePath)
        {
            stream = new MP3Stream(filePath);
        }

        public AudioProperties ReadProperties()
        {
            return new()
            {
                NumChannels = stream.ChannelCount,
                SampleRate = stream.Frequency,
                BitsPerSample = 16,
            };
        }

        public byte[] GetAudioData()
        {
            using var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
        
        public void Dispose()
        {
            stream.Dispose();
        }
    }
}