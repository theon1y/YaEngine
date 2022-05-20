using System.Numerics;
using YaEcs;
using YaEngine.Core;
using YaEngine.ImGui;
using static ImGuiNET.ImGui;

namespace YaEngine
{
    public class ShowTransformsGuiSystem : IImGuiSystem
    {
        public void Execute(IWorld world)
        {
            Begin("Transforms");
            SetWindowSize(new Vector2(300, 500));
            
            world.ForEach((Entity entity, Transform transform) =>
            {
                var label = world.TryGetComponent(entity, out Camera _) ? "Camera: " : "Entity: ";
                LabelText(label, "");
                SameLine();
                Text(entity.Id.ToString());
                ShowTransform(transform);
            });
            
            End();
        }

        private void ShowTransform(Transform transform)
        {
            LabelText("Transform:", "");
            Text("Position");
            SameLine();
            Text(transform.Position.ToString());
            Text("Rotation");
            SameLine();
            Text(transform.Rotation.ToEulerDegrees().ToString());
            Text("Scale");
            SameLine();
            Text(transform.Scale.ToString());
        }
    }
}