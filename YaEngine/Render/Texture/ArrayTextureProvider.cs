using Silk.NET.OpenGL;

namespace YaEngine.Render
{
    public class ArrayTextureProvider : TextureProviderBase
    {
        private readonly byte[] data;
        private readonly uint width;
        private readonly uint height;

        public ArrayTextureProvider(byte[] data, uint width, uint height)
        {
            this.data = data;
            this.width = width;
            this.height = height;
        }

        public override void Load(GL gl)
        {
            unsafe
            {
                fixed (void* d = &data[0])
                {
                    //Setting the data of a texture.
                    gl.TexImage2D(TextureType, 0, (int) InternalFormat, width, height, 0,
                        SourceFormat, SourcePixelType, d);
                }
            }
            base.Load(gl);
        }
    }
}