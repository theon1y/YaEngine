using System.Numerics;
using Silk.NET.Input;

namespace YaEngine.Input.Silk
{
    public class SilkInputContextComponent : InputContext
    {
        public override IInputContext Instance { get; }
        
        public SilkInputContextComponent(IInputContext inputContext)
        {
            Instance = inputContext;
        }
        
        public override bool IsKeyDown(Key key)
        {
            foreach (var keyboard in Instance.Keyboards)
            {
                if (keyboard.IsKeyPressed(key)) return true;
            }

            return false;
        }

        public override bool IsMouseButtonDown(MouseButton button)
        {
            foreach (var mouse in Instance.Mice)
            {
                if (mouse.IsButtonPressed(button)) return true;
            }

            return false;
        }

        public override Vector2 MousePosition => Instance.Mice[0].Position;
    }
}