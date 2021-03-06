using System;
using System.Buffers.Binary;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace YaEngine.Audio
{
    public class RiffWaveAudioProvider : IAudioProvider
    {
        private const int HeaderSize = 44;
        private const int BufferSize = 1024;
        private const string HeaderStart = "RIFF";
        private const string HeaderEnd = "WAVE";

        private readonly string filePath;
        private readonly ILogger logger;
        private readonly byte[] buffer;
        
        private int headerEndOffset;

        public RiffWaveAudioProvider(string filePath, ILogger logger)
        {
            this.filePath = filePath;
            this.logger = logger;
            buffer = new byte[BufferSize];
        }

        public AudioProperties ReadProperties()
        {
            var span = new Span<byte>(buffer, 0, HeaderSize);
            using (var fileStream = File.OpenRead(filePath))
            {
                fileStream.Read(span);
            }
            
            var offset = 0;
            var headerStart = ReadString(span.Slice(offset, HeaderStart.Length));
            if (headerStart != HeaderStart)
            {
                throw new AudioFormatException($"{filePath} is not in RIFF format");
            }

            offset += HeaderStart.Length;
            offset += 4; // chunk size

            var headerEnd = ReadString(span.Slice(offset, HeaderEnd.Length));
            if (headerEnd != HeaderEnd)
            {
                throw new AudioFormatException($"{filePath} is not in WAVE format");
            }
            offset += HeaderEnd.Length;
            
            var identifier = ReadString(span.Slice(offset, 4));
            offset += 4;

            if (identifier != "fmt ")
            {
                throw new AudioFormatException($"Unknown identifier {identifier}");
            }
            
            var size = BinaryPrimitives.ReadInt32LittleEndian(new Span<byte>(buffer, offset, 4));
            offset += 4;
                
            if (size != 16)
            {
                throw new AudioFormatException($"Unknown Audio Format with subchunk size {size}");
            }

            var result = new AudioProperties();
            var audioFormat = BinaryPrimitives.ReadInt16LittleEndian(new Span<byte>(buffer, offset, 2));
            offset += 2;
            if (audioFormat != 1)
            {
                throw new AudioFormatException($"Unknown Audio Format with ID {audioFormat}");
            }

            result.NumChannels = BinaryPrimitives.ReadInt16LittleEndian(new Span<byte>(buffer, offset, 2));
            offset += 2;
            result.SampleRate = BinaryPrimitives.ReadInt32LittleEndian(new Span<byte>(buffer, offset, 4));
            offset += 4;
            // ByteRate info
            offset += 4;
            // BlockAlignInfo
            offset += 2;
            result.BitsPerSample = BinaryPrimitives.ReadInt16LittleEndian(new Span<byte>(buffer, offset, 2));
            offset += 2;

            headerEndOffset = offset;
            
            logger.LogDebug("Success. Detected RIFF-WAVE audio file, PCM encoding. {0}", result);

            return result;
        }
        
        public byte[] GetAudioData()
        {
            var bytes = File.ReadAllBytes(filePath);
            return ReadFromSpan(bytes);
        }

        public async Task<byte[]> GetAudioDataAsync(CancellationToken ct = default)
        {
            var bytes = await File.ReadAllBytesAsync(filePath, ct);
            return ReadFromSpan(bytes);
        }

        private byte[] ReadFromSpan(ReadOnlySpan<byte> span)
        {
            var index = headerEndOffset;
            while (index + 4 < span.Length)
            {
                var identifier = ReadString(span.Slice(index, 4));
                index += 4;
                var size = BinaryPrimitives.ReadInt32LittleEndian(span.Slice(index, 4));
                index += 4;
                if (identifier == "data")
                {
                    var data = span.Slice(44, size).ToArray();

                    logger.LogDebug("Read {0} bytes Data", size);
                    return data;
                }
                
                if (string.Equals(identifier, "JUNK", StringComparison.OrdinalIgnoreCase))
                {
                    // this exists to align things
                    index += size;
                }
                else if (identifier == "iXML")
                {
                    var v = span.Slice(index, size);
                    var str = ReadString(v);
                    logger.LogDebug("iXML Chunk: {0}", str);
                    index += size;
                }
                else
                {
                    logger.LogDebug("Unknown Section: {0}", identifier);
                    index += size;
                }
            }

            return new byte[0];
        }

        private static string ReadString(ReadOnlySpan<byte> buffer)
        {
            return Encoding.ASCII.GetString(buffer);
        }
        
        public void Dispose()
        {
            
        }
    }
}