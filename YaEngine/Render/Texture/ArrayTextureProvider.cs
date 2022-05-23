namespace YaEngine.Render
{
    public class ArrayTextureProvider : TextureProvider
    {
        private readonly byte[] data;

        public ArrayTextureProvider(byte[] data, uint width, uint height)
        {
            this.data = data;
            Width = width;
            Height = height;
        }

        public override byte[] Load()
        {
            return data;
        }
    }
}