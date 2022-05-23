using YaEcs;

namespace YaEngine.Render
{
    public interface IShaderFactory : IComponent
    {
        IShader Create(string name, IShaderProvider vertexShader, IShaderProvider fragmentShader);
    }
}