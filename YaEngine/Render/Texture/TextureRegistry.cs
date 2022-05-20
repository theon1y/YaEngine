using System.Collections.Generic;
using YaEcs;

namespace YaEngine.Render
{
    public class TextureRegistry : IComponent
    {
        private Dictionary<string, Texture> storage = new();

        public void Add(Texture texture)
        {
            storage.Add(texture.Name, texture);
        }

        public bool TryGet(string name, out Texture texture)
        {
            return storage.TryGetValue(name, out texture);
        }

        public IEnumerable<Texture> Textures => storage.Values;
    }
}