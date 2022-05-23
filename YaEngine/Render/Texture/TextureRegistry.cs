using System.Collections.Generic;
using YaEcs;

namespace YaEngine.Render
{
    public class TextureRegistry : IComponent
    {
        private readonly Dictionary<string, ITexture> storage = new();

        public void Add(ITexture texture)
        {
            storage.Add(texture.Name, texture);
        }

        public bool TryGet<T>(string name, out T texture) where T : GlTexture
        {
            texture = default;
            if (!storage.TryGetValue(name, out var iTexture) || iTexture is not T tTexture) return false;

            texture = tTexture;
            return true;
        }

        public IEnumerable<ITexture> Textures => storage.Values;
    }
}