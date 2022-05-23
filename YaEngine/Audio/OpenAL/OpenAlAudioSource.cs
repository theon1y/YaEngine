using System;
using Silk.NET.OpenAL;

namespace YaEngine.Audio.OpenAL
{
    public class OpenAlAudioSource : AudioSource
    {
        private readonly AL al;
        private readonly IAudioProvider audioProvider;
        private readonly uint handle;
        private readonly uint bufferId;

        public OpenAlAudioSource(AL al, IAudioProvider audioProvider)
        {
            this.al = al;
            this.audioProvider = audioProvider;
            handle = al.GenSource();
            bufferId = al.GenBuffer();
        }

        public void Load()
        {
            var properties = audioProvider.ReadProperties();
            var format = GetAlBufferFormat(properties);
            unsafe
            {
                var data = audioProvider.GetAudioData();
                fixed(byte* pData = data)
                    al.BufferData(bufferId, format, pData, data.Length, properties.SampleRate);
            }
        }

        public override void Play(bool isLooping)
        {
            al.SetSourceProperty(handle, SourceBoolean.Looping, isLooping);
            al.SetSourceProperty(handle, SourceInteger.Buffer, bufferId);
            al.SourcePlay(handle);
            IsPlaying = true;
        }

        public override void Stop()
        {
            IsPlaying = false;
            al.SourceStop(handle);
        }

        public override void Dispose()
        {
            Stop();
            al.DeleteSource(handle);
            al.DeleteBuffer(bufferId);
            audioProvider.Dispose();
        }

        private static BufferFormat GetAlBufferFormat(AudioProperties properties)
        {
            return (properties.NumChannels, properties.BitsPerSample) switch
            {
                (1, 8) => BufferFormat.Mono8,
                (1, 16) => BufferFormat.Mono16,
                (2, 8) => BufferFormat.Stereo8,
                (2, 16) => BufferFormat.Stereo16,
                _ => throw new ArgumentException(
                    $"Can not play {properties.NumChannels} channels {properties.BitsPerSample} bit format")
            };
        }
    }
}