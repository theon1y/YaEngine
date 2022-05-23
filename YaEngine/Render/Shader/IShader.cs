using System;

namespace YaEngine.Render
{
    public interface IShader : IDisposable
    {
        string Name { get; }
        void Use();
        bool TryGetUniformLocation(string name, out int location);
        bool TryGetAttributeLocation(string name, out int location);
        void SetUniform<T>(int location, T value);
    }
}