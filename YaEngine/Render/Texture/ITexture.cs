using System;

namespace YaEngine.Render
{
    public interface ITexture : IDisposable
    {
        public string Name { get; }
        public void Bind(int slot);
    }
}