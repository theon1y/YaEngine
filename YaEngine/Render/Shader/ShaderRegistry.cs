using System.Collections.Generic;
using YaEcs;

namespace YaEngine.Render
{
    public class ShaderRegistry : IComponent
    {
        private Dictionary<string, Shader> storage = new();

        public void Add(Shader shader)
        {
            storage.Add(shader.Name, shader);
        }

        public bool TryGet(string name, out Shader shader)
        {
            return storage.TryGetValue(name, out shader);
        }

        public IEnumerable<Shader> Shaders => storage.Values;
    }
}