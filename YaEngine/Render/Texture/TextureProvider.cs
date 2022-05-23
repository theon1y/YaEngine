namespace YaEngine.Render
{
    public abstract class TextureProvider
    {
        public TextureWrap Wrap = TextureWrap.Repeat;
        public TextureFilter Filter = TextureFilter.Linear;
        public bool UseMipMaps = false;
        public TextureType Type = TextureType.Texture2d;
        public TextureFormat Format = TextureFormat.Rgba;
        public PixelFormat SourceFormat = PixelFormat.Rgba;
        public PixelType SourcePixelType = PixelType.UnsignedByte;
        
        public uint Width { get; protected set; }
        public uint Height { get; protected set; }
        public abstract byte[] Load();
    }
}