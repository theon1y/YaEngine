using System.Numerics;
using Silk.NET.Input;
using YaEcs;

namespace YaEngine.Input
{
    public abstract class InputContext : IComponent
    {
        public abstract IInputContext Instance { get; }
        public abstract bool IsKeyDown(Key key);
        public abstract bool IsMouseButtonDown(MouseButton button);
        public abstract Vector2 MousePosition { get; }

        public Vector2 PrevMousePosition;
        public Vector2 MouseDelta;
    }
}