using System.Numerics;
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

            if (input.PrevMousePosition != Vector2.Zero)
            {
                input.MouseDelta = input.MousePosition - input.PrevMousePosition;
            }
            input.PrevMousePosition = input.MousePosition;
        }
    }
}