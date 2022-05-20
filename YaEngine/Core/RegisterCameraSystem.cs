using YaEcs;
using YaEcs.Bootstrap;

namespace YaEngine.Core
{
    public class RegisterCameraSystem : IUpdateSystem
    {
        public UpdateStep UpdateStep => UpdateSteps.EarlyUpdate;
        
        public void Execute(IWorld world)
        {
            if (!world.TryGetSingleton(out CameraRegistry registry)) return;

            registry.Cameras.Clear();
            world.ForEach((Entity entity, Camera _) =>
            {
                registry.Cameras.Add(entity);
            });
        }
    }
}