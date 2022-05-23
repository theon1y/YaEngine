using System.Collections.Generic;
using System.Numerics;
using Silk.NET.Input;
using YaEcs;

namespace YaEngine.Input
{
    public abstract class InputContext : IComponent
    {
        public HashSet<Key> PressedKeys = new(10);
        public HashSet<Key> HoldKeys = new(10);
        public HashSet<Key> ReleasedKeys = new(10);
        
        public abstract IInputContext Instance { get; }
        public abstract bool IsKeyPressed(Key key);
        public abstract bool IsMouseButtonPressed(MouseButton button);
        public abstract Vector2 MousePosition { get; }

        public Vector2 PrevMousePosition;
        public Vector2 MouseDelta;

        public bool IsKeyDown(Key key)
        {
            return PressedKeys.Contains(key);
        }

        public bool IsKeyUp(Key key)
        {
            return ReleasedKeys.Contains(key);
        }
    }
}