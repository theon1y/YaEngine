using System;
using System.Threading;
using System.Threading.Tasks;
using Silk.NET.OpenAL;

namespace YaEngine.Audio.OpenAL
{
    public class OpenAlAudioSource : AudioSource
    {
        private readonly CancellationTokenSource cts;
        private readonly AL al;
        private readonly IAudioProvider audioProvider;
        private readonly uint handle;
        private readonly uint bufferId;
        private byte[]? data;

        public OpenAlAudioSource(AL al, IAudioProvider audioProvider)
        {
            this.al = al;
            this.audioProvider = audioProvider;
            handle = al.GenSource();
            bufferId = al.GenBuffer();
            cts = new CancellationTokenSource();
        }

        public override void Play(bool isLooping)
        {
            IsPlaying = true;
            if (data != null)
            {
                PlayFromBuffer(isLooping, data);
                return;
            }
            
            Task.Run(LoadAsync)
                .ContinueWith(task => OnLoad(task, isLooping));
        }

        private async Task<byte[]> LoadAsync()
        {
            data = await audioProvider.GetAudioDataAsync(cts.Token);
            return data;
        }

        private void OnLoad(Task<byte[]> loadTask, bool isLooping)
        {
            if (loadTask.IsCanceled || loadTask.IsFaulted)
            {
                Dispose();
                return;
            }
            
            PlayFromBuffer(isLooping, loadTask.Result);
        }

        private void PlayFromBuffer(bool isLooping, byte[] buffer)
        {
            var properties = audioProvider.ReadProperties();
            var format = GetAlBufferFormat(properties);
            al.BufferData(bufferId, format, buffer, properties.SampleRate);
            al.SetSourceProperty(handle, SourceBoolean.Looping, isLooping);
            al.SetSourceProperty(handle, SourceInteger.Buffer, bufferId);
            al.SourcePlay(handle);
        }

        public override void Stop()
        {
            IsPlaying = false;
            al.SourceStop(handle);
        }

        public override void Dispose()
        {
            Stop();
            cts.Cancel();
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