using Silk.NET.Maths;

namespace YaEngine.Bootstrap
{
    public interface IApplication
    {
        Vector2D<int> Size { get; }
        void Quit();
    }
}