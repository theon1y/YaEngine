using System.Collections.Generic;
using YaEcs;

namespace YaEngine.Render
{
    public class ShaderRegistry : IComponent
    {
        private readonly Dictionary<string, IShader> storage = new();

        public void Add(IShader shader)
        {
            storage.Add(shader.Name, shader);
        }

        public bool TryGet<T>(string name, out T shader) where T : IShader
        {
            shader = default;
            if (!storage.TryGetValue(name, out var iShader) || iShader is not T tShader) return false;

            shader = tShader;
            return true;
        }

        public IEnumerable<IShader> Shaders => storage.Values;
    }
}