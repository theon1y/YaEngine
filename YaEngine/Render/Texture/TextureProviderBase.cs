using Silk.NET.OpenGL;

namespace YaEngine.Render
{
    public abstract class TextureProviderBase : ITextureProvider
    {
        public TextureWrap Wrap = TextureWrap.Repeat;
        public TextureFilter Filter = TextureFilter.Linear;
        public bool UseMipMaps = false;
        public TextureTarget TextureType = TextureTarget.Texture2D;
        public InternalFormat InternalFormat = InternalFormat.Rgba;
        public PixelFormat SourceFormat = PixelFormat.Rgba;
        public PixelType SourcePixelType = PixelType.UnsignedByte;
        
        public virtual void Load(GL gl)
        {
            SetParameters(gl);
        }
        
        private void SetParameters(GL gl)
        {
            gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) Wrap);
            gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) Wrap);
            gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) Filter);
            gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) Filter);
            if (UseMipMaps)
            {
                gl.GenerateMipmap(TextureTarget.Texture2D);
            }
        }
    }
}