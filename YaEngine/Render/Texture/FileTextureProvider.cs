using Silk.NET.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace YaEngine.Render
{
    public class FileTextureProvider : TextureProviderBase
    {
        private readonly string path;

        public FileTextureProvider(string path)
        {
            this.path = path;
        }

        public override void Load(GL gl)
        {
            using var img = Image.Load<Rgba32>(path);
            unsafe
            {
                //Reserve enough memory from the gpu for the whole image
                gl.TexImage2D(TextureType, 0, InternalFormat, (uint) img.Width, (uint) img.Height,
                    0, SourceFormat, SourcePixelType, null);

                img.ProcessPixelRows(accessor =>
                {
                    //ImageSharp 2 does not store images in contiguous memory by default, so we must send the image row by row
                    for (var y = 0; y < accessor.Height; y++)
                    {
                        fixed (void* data = accessor.GetRowSpan(y))
                        {
                            //Loading the actual image.
                            gl.TexSubImage2D(TextureType, 0, 0, accessor.Height-y-1, (uint) accessor.Width,
                                1, SourceFormat, SourcePixelType, data);
                        }
                    }
                });
            }
            base.Load(gl);
        }
    }
}