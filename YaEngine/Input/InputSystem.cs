using System.Numerics;
using Silk.NET.Input;
using Silk.NET.Input.Extensions;
using YaEcs;
using YaEcs.Bootstrap;

namespace YaEngine.Input
{
    public class InputSystem : IUpdateSystem
    {
        public UpdateStep UpdateStep => UpdateSteps.First;
        
        public void Execute(IWorld world)
        {
            if (!world.TryGetSingleton(out InputContext input)) return;

            if (input.IsKeyPressed(Key.AltLeft))
            {
                input.Instance.Mice[0].Cursor.CursorMode = CursorMode.Normal;
                input.MouseDelta = Vector2.Zero;
            }
            else
            {
                input.Instance.Mice[0].Cursor.CursorMode = CursorMode.Raw;
                if (input.PrevMousePosition != Vector2.Zero)
                {
                    input.MouseDelta = input.MousePosition - input.PrevMousePosition;
                }
            }
            input.PrevMousePosition = input.MousePosition;
            
            input.PressedKeys.Clear();
            input.ReleasedKeys.Clear();
            input.ReleasedKeys.UnionWith(input.HoldKeys);
            input.HoldKeys.Clear();
            foreach (var keyboard in input.Instance.Keyboards)
            {
                using var state = keyboard.CaptureState();
                foreach (var key in state.GetPressedKeys())
                {
                    input.HoldKeys.Add(key);
                }
            }
            var prevPressed = input.ReleasedKeys;
            var currentPressed = input.HoldKeys;
            input.PressedKeys.UnionWith(currentPressed);
            input.PressedKeys.ExceptWith(prevPressed);
            prevPressed.ExceptWith(currentPressed);
        }
    }
}