using System;
using System.Runtime.InteropServices;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace YaEngine.Render
{
    public class FileTextureProvider : TextureProvider
    {
        private readonly string path;

        public FileTextureProvider(string path)
        {
            this.path = path;
        }

        public override byte[] Load()
        {
            using var img = Image.Load<Rgba32>(path);
            Width = (uint) img.Width;
            Height = (uint) img.Height;
            int size;
            
            unsafe
            {
                size = sizeof(Rgba32);
            }
            
            var result = new byte[Width * Height * size];
            var rowLength = (int) Width * size;
            img.ProcessPixelRows(accessor =>
            {
                //ImageSharp 2 does not store images in contiguous memory by default, so we must send the image row by row
                for (var y = 0; y < accessor.Height; y++)
                {
                    // image is processed upside down
                    var start = (accessor.Height - y - 1) * rowLength;
                    var target = new Span<byte>(result, start, rowLength);
                    var rowSpan = MemoryMarshal.Cast<Rgba32, byte>(accessor.GetRowSpan(y));
                    rowSpan.CopyTo(target);
                }
            });
            return result;
        }
    }
}