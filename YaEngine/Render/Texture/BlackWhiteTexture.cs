namespace YaEngine.Render
{
    public class BlackWhiteTexture
    {
        private const uint Width = 4;
        private const uint Height = 4;
        private static byte[] data =
        {
            255, 255, 255, 255,
            255, 255, 255, 255,
            0, 0, 0, 0,
            0, 0, 0, 0,
            
            255, 255, 255, 255,
            255, 255, 255, 255,
            0, 0, 0, 0,
            0, 0, 0, 0,
            
            0, 0, 0, 0,
            0, 0, 0, 0,
            255, 255, 255, 255,
            255, 255, 255, 255,
            
            0, 0, 0, 0,
            0, 0, 0, 0,
            255, 255, 255, 255,
            255, 255, 255, 255,
        };

        public static Texture Value = new(nameof(BlackWhiteTexture), new ArrayTextureProvider(data, Width, Height)
        {
            Filter = TextureFilter.Nearest
        });
    }
}