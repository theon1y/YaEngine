using System.Drawing;
using YaEcs;

namespace YaEngine.Render
{
    public abstract class RenderApi : IComponent
    {
        public Color ClearColor { get; set; }
        public abstract void Clear();
        public abstract void Draw(Renderer renderer);
        public abstract void PrepareOpaqueRender();
        public abstract void PrepareTransparentRender();
    }
}