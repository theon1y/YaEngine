using Silk.NET.Maths;
using Silk.NET.Windowing;

namespace YaEngine.Bootstrap.Silk
{
    public class SilkApplication : IApplication
    {
        private readonly IWindow window;

        public SilkApplication(IWindow window)
        {
            this.window = window;
        }

        public Vector2D<int> Size => window.Size;

        public void Quit()
        {
            window.Close();
        }
    }
}