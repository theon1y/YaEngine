using YaEcs;

namespace YaEngine.Render
{
    public interface ITextureFactory : IComponent
    {
        ITexture Create(string name, TextureProvider provider);
    }
}